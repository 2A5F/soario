#pragma once
#include <atomic>
#include <memory>
#include <string>
#include <SDL3/SDL.h>

#include "../pch.h"

namespace ccc {
    class Window;
    class RenderContext;

    class WindowSystem {
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

    class Window {
        SDL_Window *m_window{};
        std::shared_ptr<RenderContext> m_render_context{};

    public:
        static WindowBuilder builder();

        static std::shared_ptr<Window> create(const WindowOptions &options);

        ~Window();

        // 获取或创建渲染上下文
        const std::shared_ptr<RenderContext> &render_context();

        HWND hwnd() const;

        float2 size() const;
    };
} // ccc
