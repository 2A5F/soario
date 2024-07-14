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

        void assert_same_thread() const {
            if (!check_same_thread())
                throw std::exception("Multithreaded calls to single-threaded objects");
        }
    };
} // ccc
