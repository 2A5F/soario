#pragma once
#include <atomic>
#include <memory>
#include <string>
#include <SDL3/SDL.h>

#include "../pch.h"
#include "../api/FWindow.h"
#include "../utils/Object.h"
#include "../utils/Rc.h"
#include "../utils/String.h"

#include "parallel_hashmap/phmap.h"

namespace ccc {
    class Window;
    class RenderContext;

    class WindowHandle final : public virtual Object {
        SDL_Window *m_window{};
        void *m_gc_handle{};

    public:
        explicit WindowHandle(SDL_Window *window);

        ~WindowHandle() override;

        void set_gc_handle(void *handle);

        void *gc_handle() const;

        uint32_t id() const;

        HWND hwnd() const;

        int2 size() const;

        void close() const;
    };

    enum class SoarMsgEvent {
        // 无操作，忽略
        None = 0,
        // 创建窗口 data1 包含窗口创建参数
        CreateWindow = 1,
    };

    struct WindowCreateParamPack {
        String title;
        int2 size;
        std::optional<int2> min_size;
        Rc<Window> window;
        SDL_Semaphore *semaphore;

        void create();
    };

    class WindowSystem final : public virtual Object {
        friend Window;

        std::atomic_bool m_exited{false};

        int m_exit_code{0};

        phmap::parallel_flat_hash_map<uint32_t, Weak<Window> > m_windows{};

        explicit WindowSystem() {
        }

    public:
        static void init();

        static const WindowSystem &instance();

        static int main_loop();

        static bool is_exited();

        static void exit(int code);
    };

    struct WindowOptions {
        const char *title;
        int2 size;
        std::optional<int2> min_size;
    };

    struct WindowBuilder {
        WindowOptions options;

        WindowBuilder &title(const char *title) {
            options.title = title;
            return *this;
        }

        WindowBuilder &size(const int32_t width, const int32_t height) {
            options.size = int2(width, height);
            return *this;
        }

        WindowBuilder &min_size(const int32_t width, const int32_t height) {
            options.min_size = int2(width, height);
            return *this;
        }

        Rc<Window> build() const;
    };

    class Window final : public FWindow {
        IMPL_RC(Window)

        friend WindowSystem;

        std::shared_ptr<WindowHandle> m_inner{};
        std::shared_ptr<RenderContext> m_render_context{};

        bool m_resized{false};

    public:
        static WindowBuilder builder();

        static Rc<Window> create(const WindowOptions &options);

        // 获取或创建渲染上下文
        const std::shared_ptr<RenderContext> &render_context();

        const std::shared_ptr<WindowHandle> &inner() const;

        HWND hwnd() const;

        int2 size() const;

        // 大小是否改变
        bool resized() const;

        // 消除大小改变事件
        void use_resize();

        void set_gc_handle(void *handle) override;

        void *gc_handle() const;
    };
} // ccc
