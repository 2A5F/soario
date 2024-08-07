#pragma once
#include <atomic>
#include <memory>
#include <string>
#include <SDL3/SDL.h>

#include "../pch.h"
#include "../ffi/FWindow.h"
#include "../utils/Rc.h"
#include "../utils/String.h"

#include "parallel_hashmap/phmap.h"

namespace ccc
{
    class Window;

    class WindowHandle final : FObject
    {
        IMPL_RC(WindowHandle);

        SDL_Window* m_window{};
        void* m_gc_handle{};

    public:
        explicit WindowHandle(SDL_Window* window);

        ~WindowHandle() override;

        void set_gc_handle(void* handle);

        void* gc_handle() const;

        uint32_t id() const;

        HWND hwnd() const;

        int2 size() const;

        void get_size(FError* err, FInt2* size) const;
    };

    enum class SoarMsgEvent
    {
        // 无操作，忽略
        None = 0,
        // 创建窗口 data1 包含窗口创建参数
        CreateWindow = 1,
    };

    struct WindowCreateParamPack
    {
        Rc<String> title;
        int2 size;
        std::optional<int2> min_size;
        Rc<Window> window;
        SDL_Semaphore* semaphore;

        void create();
    };

    class WindowSystem final : FObject
    {
        IMPL_RC(WindowSystem);

        friend Window;

        std::atomic_bool m_exited{false};

        int m_exit_code{0};

        phmap::parallel_flat_hash_map<uint32_t, Weak<Window>> m_windows{};

        explicit WindowSystem()
        {
        }

    public:
        static void init();

        static const WindowSystem& instance();

        static int main_loop();

        static bool is_exited();

        static void exit(int code);
    };

    struct WindowOptions
    {
        const char* title;
        int2 size;
        std::optional<int2> min_size;
    };

    struct WindowBuilder
    {
        WindowOptions options;

        WindowBuilder& title(const char* title)
        {
            options.title = title;
            return *this;
        }

        WindowBuilder& size(const int32_t width, const int32_t height)
        {
            options.size = int2(width, height);
            return *this;
        }

        WindowBuilder& min_size(const int32_t width, const int32_t height)
        {
            options.min_size = int2(width, height);
            return *this;
        }

        Rc<Window> build() const;
    };

    class Window final : public FWindow
    {
        IMPL_RC(Window)

        friend WindowSystem;

        Rc<WindowHandle> m_inner{};

        bool m_resized{false};

    public:
        static WindowBuilder builder();

        static Rc<Window> create(const WindowOptions& options);

        const Rc<WindowHandle>& inner() const;

        HWND hwnd() const;

        int2 size() const;

        size_t get_hwnd() noexcept override;

        // 大小是否改变
        bool resized() const;

        // 消除大小改变事件
        void use_resize();

        void set_gc_handle(void* handle) noexcept override;

        void* gc_handle() const;

        void get_size(FError* err, FInt2* size) const noexcept override;
    };
} // ccc
