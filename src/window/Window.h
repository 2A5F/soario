#pragma once
#include <atomic>
#include <memory>
#include <string>
#include <SDL3/SDL.h>

#include "../pch.h"
#include "../utils/IObject.h"

namespace ccc {
    class Window;
    class RenderContext;

    class WindowHandle final : public IObject {
        SDL_Window *m_window{};

    public:
        explicit WindowHandle(SDL_Window *window);

        ~WindowHandle() override;

        HWND hwnd() const;

        float2 size() const;
    };

    class WindowSystem final : public IObject {
        std::atomic_bool m_exited{false};

        explicit WindowSystem() {
        }

    public:
        static void init();

        static const WindowSystem &instance();

        static int main_loop();

        static bool is_exited();
    };

    struct WindowOptions {
        std::string title;
        int2 size;
        std::optional<int2> min_size;
    };

    struct WindowBuilder {
        WindowOptions options;

        WindowBuilder &title(std::string &&title) {
            options.title = std::move(title);
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

        std::shared_ptr<Window> build() const;
    };

    class Window final : public IObject {
        std::shared_ptr<WindowHandle> m_inner{};
        std::shared_ptr<RenderContext> m_render_context{};

    public:
        static WindowBuilder builder();

        static std::shared_ptr<Window> create(const WindowOptions &options);

        // 获取或创建渲染上下文
        const std::shared_ptr<RenderContext> &render_context();

        const std::shared_ptr<WindowHandle> &inner() const;

        HWND hwnd() const;

        float2 size() const;
    };
} // ccc
