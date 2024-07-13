#include "Window.h"

#include <memory>
#include <SDL3/SDL.h>

#include "dwmapi.h"
#include "../render/RenderContext.h"

#include "../utils/sdl_error.h"

namespace ccc {
    namespace {
        std::unique_ptr<WindowSystem> s_instance;

        void SetMica(HWND hwnd) {
            uint32_t value = 4;
            auto r = DwmSetWindowAttribute(hwnd, DWMWA_SYSTEMBACKDROP_TYPE, &value, sizeof uint32_t);
            if (r != 0) {
                value = 1;
                DwmSetWindowAttribute(hwnd, 1029, &value, sizeof uint32_t);
            }

            MARGINS margins{-1, -1, -1, -1};
            DwmExtendFrameIntoClientArea(hwnd, &margins);
        }
    }

    void WindowSystem::init() {
        s_instance = std::unique_ptr<WindowSystem>(new WindowSystem());
        if (SDL_Init(SDL_INIT_VIDEO)) {
            throw new sdl_error();
        }
    }

    const WindowSystem &WindowSystem::instance() {
        return *s_instance;
    }

    int WindowSystem::main_loop() {
        while (!s_instance->m_exited.load()) {
            SDL_Event event;
            if (SDL_WaitEvent(&event)) {
                if (event.type == SDL_EVENT_QUIT) {
                    s_instance->m_exited.store(true);
                }
            } else {
                throw sdl_error();
            }
        }
        return 0;
    }

    bool WindowSystem::is_exited() {
        return s_instance->m_exited.load();
    }

    std::shared_ptr<Window> WindowBuilder::build() const {
        return Window::create(options);
    }

    WindowBuilder Window::builder() {
        return WindowBuilder();
    }

    std::shared_ptr<Window> Window::create(const WindowOptions &options) {
        const auto sw = SDL_CreateWindow(
            options.title.c_str(), options.size.x, options.size.y,
            SDL_WINDOW_RESIZABLE
        );
        if (!sw) {
            throw sdl_error();
        }
        if (options.min_size.has_value()) {
            const auto size = options.min_size.value();
            if (const auto r = SDL_SetWindowMinimumSize(sw, size.x, size.y); r != 0)
                throw sdl_error();
        }
        auto win = std::make_shared<Window>();
        win->m_window = sw;
        const auto hwnd = win->hwnd();
        SetMica(hwnd);
        return win;
    }

    Window::~Window() {
        SDL_DestroyWindow(m_window);
    }

    const std::shared_ptr<RenderContext> &Window::render_context() {
        if (m_render_context == nullptr) {
            m_render_context = RenderContext::create(*this);
        }
        return m_render_context;
    }

    HWND Window::hwnd() const {
        const auto props = SDL_GetWindowProperties(m_window);
        if (props == 0) throw sdl_error();
        const auto hwnd = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_WIN32_HWND_POINTER, nullptr);
        return static_cast<HWND>(hwnd);
    }

    float2 Window::size() const {
        int w, h;
        if (0 != SDL_GetWindowSizeInPixels(m_window, &w, &h)) {
            throw sdl_error();
        }
        return float2(w, h);
    }
} // ccc
