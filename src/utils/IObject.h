#pragma once
#include <thread>

namespace ccc {
    struct IObject {
        virtual ~IObject() = default;
    };

    // 单线程对象，不可跨线程调用
    struct ISingleThreadObject : IObject {
    protected:
        std::thread::id m_thread_id;

    public:
        explicit ISingleThreadObject() : m_thread_id(std::this_thread::get_id()) {
        }

        bool check_same_thread() const {
            return std::this_thread::get_id() == m_thread_id;
        }

        bool check_same_thread(const std::thread::id other) const {
            return other == m_thread_id;
        }

        void assert_same_thread() const {
            if (!check_same_thread())
                throw std::exception("Multithreaded calls to single-threaded objects");
        }

        void assert_same_thread(const std::thread::id other) const {
            if (!check_same_thread(other))
                throw std::exception("Multithreaded calls to single-threaded objects");
        }

        // 在当前线程上获取所有权
        void unsafe_take_thread_ownership() {
            m_thread_id = std::this_thread::get_id();
        }

        // 转移所有权到别的线程
        void transfer_thread_ownership(const std::thread::id other) {
            assert_same_thread();
            m_thread_id = other;
        }
    };
} // ccc
