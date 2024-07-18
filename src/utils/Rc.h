#pragma once

#define IMPL_RC(Name) \
    template<class T> \
    friend class Rc; \
    \
    template<class T> \
    friend class Weak; \
    \
    mutable std::atomic_size_t m_strong{1}; \
    mutable std::atomic_size_t m_weak{1}; \
    \
public: \
    size_t AddRef() const override { \
        return strong_clone(); \
    } \
    \
    size_t Release() override { \
        return strong_drop(); \
    } \
    \
private: \
    size_t strong_clone() const { \
        return m_strong.fetch_add(1, std::memory_order_relaxed); \
    } \
    \
    size_t weak_clone() const { \
        return m_weak.fetch_add(1, std::memory_order_relaxed); \
    } \
    \
    size_t strong_drop() { \
        const size_t r = m_strong.fetch_sub(1, std::memory_order_release); \
        if (r != 1) return r; \
    \
        drop_slow(); \
        return r; \
    } \
    \
    void drop_slow() { \
        this->~Name(); \
     \
        weak_drop(); \
    } \
    \
    void weak_drop() { \
        if (m_weak.fetch_sub(1, std::memory_order_release) == 1) { \
            operator delete(this); \
        } \
    } \
    \
    bool try_upgrade() const { \
        size_t cur = m_strong.load(std::memory_order_relaxed); \
    re_try: \
        if (cur == 0) return false; \
        if (m_strong.compare_exchange_weak(cur, cur + 1, std::memory_order_acquire, std::memory_order_relaxed)) { \
            return true; \
        } \
        goto re_try; \
    } \
    \
    bool try_downgrade() const { \
        size_t cur = m_weak.load(std::memory_order_relaxed); \
    re_try: \
        if (cur == 0) return false; \
        if (m_weak.compare_exchange_weak(cur, cur + 1, std::memory_order_acquire, std::memory_order_relaxed)) { \
            return true; \
        } \
        goto re_try; \
    }

namespace ccc {
    template<class T>
    class Weak;

    template<class T>
    class Rc final {
        T *m_ptr;

        template<class U>
        friend class Rc;

        template<class U>
        friend class Weak;

        struct clone_t {
        };

        struct upgrade_t {
        };

        // clone
        explicit Rc(T *ptr, clone_t) : m_ptr(ptr) {
            static_assert(std::is_base_of_v<FObject, T>);

            if (auto p = get()) {
                p->strong_clone();
            }
        }

        // upgrade
        explicit Rc(T *ptr, upgrade_t) : m_ptr(ptr) {
            static_assert(std::is_base_of_v<FObject, T>);

            if (auto p = get()) {
                if (!p->try_upgrade()) {
                    this->m_ptr = nullptr;
                    return;
                }
            }
        }

        void drop() {
            if (auto p = m_ptr) {
                p->strong_drop();
            }
        }

    public:
        ~Rc() {
            drop();
        }

        void reset() {
            drop();
        }

        explicit operator bool() const {
            return get() != nullptr;
        }

        T *get() const {
            return m_ptr;
        }

        T &operator*() const {
            return *get();
        }

        T *operator->() const {
            return get();
        }

        // null
        Rc() : m_ptr(nullptr) {
        }

        // create
        Rc(T *ptr) : m_ptr(ptr) {
            static_assert(std::is_base_of_v<FObject, T>);
        }

        // copy
        Rc(const Rc &r) : Rc(r.get(), clone_t()) {
        }

        // copy conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Rc(const Rc<U> &r): Rc(r.get(), clone_t()) {
        }

        // move
        Rc(Rc &&r) noexcept: m_ptr(r.m_ptr) {
            r.m_ptr = nullptr;
        }

        // move conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Rc(Rc<U> &&r): m_ptr(r.m_ptr) {
            r.m_ptr = nullptr;
        }

        Rc &operator=(T *p) {
            drop();
            new(this) Rc(p);
            return *this;
        }

        // copy ass
        Rc &operator=(Rc const &r) noexcept {
            drop();
            new(this) Rc(r);
            return *this;
        }

        // move ass
        Rc &operator=(Rc &&r) noexcept {
            if (this != &r) {
                drop();
                new(this) Rc(std::forward<Rc>(r));
            }
            return *this;
        }

        // copy ass conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Rc &operator=(Rc<U> const &r) noexcept {
            drop();
            new(this) Rc(r);
            return *this;
        }

        // move ass conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Rc &operator=(Rc<U> &&r) noexcept {
            drop();
            new(this) Rc(std::forward<Rc<U> >(r));
            return *this;
        }

        Rc clone() const {
            return Rc(get(), clone_t());
        }

        static Rc UnsafeClone(T *ptr) {
            return Rc(ptr, clone_t());
        }

        static Rc UnsafeCreate(T *ptr) {
            Rc r = ptr;
            return r;
        }

        static void UnsafeDrop(T *ptr) {
            Rc _ = ptr;
        }

        // 直接泄漏，脱离 RAII 管理
        template<class Self>
        T *leak(this Self &self) {
            auto s = std::move(self);
            if (s) s->AddRef();
            return s.get();
        }

        Weak<T> downgrade() const {
            return Weak(get(), Weak<T>::downgrade_t());
        }
    };

    template<class T>
    class Weak final {
        T *m_ptr;

        template<class U>
        friend class Weak;

        template<class U>
        friend class Rc;

        struct clone_t {
        };

        struct downgrade_t {
        };

        // clone
        explicit Weak(T *ptr, clone_t) : m_ptr(ptr) {
            static_assert(std::is_base_of_v<FObject, T>);

            if (auto p = get()) {
                p->weak_clone();
            }
        }

        // downgrade
        explicit Weak(T *ptr, downgrade_t) : m_ptr(ptr) {
            static_assert(std::is_base_of_v<FObject, T>);

            if (auto p = get()) {
                if (!p->try_downgrade()) {
                    this->m_ptr = nullptr;
                    return;
                }
            }
        }

        void drop() {
            if (auto p = this->m_ptr) {
                p->weak_drop();
            }
        }

    public:
        ~Weak() {
            drop();
        }

        void reset() {
            drop();
        }

        explicit operator bool() const {
            return get() != nullptr;
        }

        // null
        Weak() : m_ptr(nullptr) {
        }

        // copy
        Weak(const Weak &r) : Weak(r.get(), clone_t()) {
        }

        // downgrade
        Weak(const Rc<T> &r) : Weak(r.get(), downgrade_t()) {
        }

        // copy conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Weak(const Weak<U> &r): Weak(r.get(), clone_t()) {
        }

        // downgrade conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Weak(const Rc<U> &r): Weak(r.get(), downgrade_t()) {
        }

        // move
        Weak(Weak &&r) noexcept: m_ptr(r.m_ptr) {
            r.m_ptr = nullptr;
        }

        // move conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Weak(Weak<U> &&r): m_ptr(r.ptr) {
            r.ptr = nullptr;
        }

        Weak &operator=(T *p) {
            drop();
            new(this) Weak(p);
            return *this;
        }

        // copy ass
        Weak &operator=(Weak const &r) noexcept {
            drop();
            new(this) Weak(r);
            return *this;
        }

        // move ass
        Weak &operator=(Weak &&r) noexcept {
            if (this != &r) {
                drop();
                new(this) Weak(std::forward<Weak>(r));
            }
            return *this;
        }

        // copy ass conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Weak &operator=(Weak<U> const &r) noexcept {
            drop();
            new(this) Weak(r);
            return *this;
        }

        // move ass conv
        template<typename U>
            requires std::convertible_to<U *, T *>
        Weak &operator=(Weak<U> &&r) noexcept {
            drop();
            new(this) Weak(std::forward<Weak<U> >(r));
            return *this;
        }

        T *get() const {
            return m_ptr;
        }

        T &operator*() const {
            return *get();
        }

        T *operator->() const {
            return get();
        }

        Rc<T> upgrade() const {
            return Rc(get(), Rc<T>::upgrade_t());
        }
    };
}
