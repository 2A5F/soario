#include "Window.h"

#include <memory>
#include <SDL3/SDL.h>

#include "dwmapi.h"
#include "../App.h"
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

    WindowHandle::WindowHandle(SDL_Window *window) : m_window(window) {
    }

    WindowHandle::~WindowHandle() {
        if (const auto window = std::exchange(m_window, nullptr)) {
            SDL_DestroyWindow(window);
        }
    }

    void WindowHandle::set_gc_handle(void *handle) {
        m_gc_handle = handle;
    }

    void *WindowHandle::gc_handle() const {
        return m_gc_handle;
    }

    uint32_t WindowHandle::id() const {
        if (!m_window) throw std::exception("m_window is null");
        const auto id = SDL_GetWindowID(m_window);
        if (id == 0) throw sdl_error();
        return id;
    }

    HWND WindowHandle::hwnd() const {
        if (!m_window) throw std::exception("m_window is null");
        const auto props = SDL_GetWindowProperties(m_window);
        if (props == 0) throw sdl_error();
        const auto hwnd = SDL_GetPointerProperty(props, SDL_PROP_WINDOW_WIN32_HWND_POINTER, nullptr);
        return static_cast<HWND>(hwnd);
    }

    int2 WindowHandle::size() const {
        if (!m_window) throw std::exception("m_window is null");
        int w, h;
        if (0 != SDL_GetWindowSizeInPixels(m_window, &w, &h)) {
            throw sdl_error();
        }
        return int2(w, h);
    }

    void WindowCreateParamPack::create() {
        auto builder = Window::builder()
            .title(title.c_str())
            .size(size.x, size.y);
        if (min_size.has_value()) builder.min_size(min_size.value().x, min_size.value().y);
        window = builder.build();
        if (SDL_PostSemaphore(semaphore) != 0) throw sdl_error();
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
        SDL_AddEventWatch([](void *data, SDL_Event *event) {
            if (event->type == SDL_EVENT_WINDOW_RESIZED) {
                if (const auto win = s_instance->m_windows[event->window.windowID].upgrade()) {
                    win->m_resized = true;
                    app_fn_vtb().window_event_handle(win->gc_handle(), FWindowEventType::Resize, nullptr);
                }
            } else if (event->type == SDL_EVENT_WINDOW_CLOSE_REQUESTED) {
                if (const auto win = s_instance->m_windows[event->window.windowID].upgrade()) {
                    app_fn_vtb().window_event_handle(win->gc_handle(), FWindowEventType::Close, nullptr);
                }
            }
            return 0;
        }, nullptr);

        while (!s_instance->m_exited.load()) {
            SDL_Event event;
            if (SDL_WaitEventTimeout(&event, 100)) {
                if (event.type == SDL_EVENT_QUIT) {
                    s_instance->m_exited.store(true);
                } else if (event.type == SDL_EVENT_WINDOW_DESTROYED) {
                    s_instance->m_windows.erase(event.window.windowID);
                } else if (event.type == SDL_EVENT_USER) {
                    switch (static_cast<SoarMsgEvent>(event.user.code)) {
                        case SoarMsgEvent::CreateWindowW: {
                            const auto pack = static_cast<WindowCreateParamPack *>(event.user.data1);
                            pack->create();
                            break;
                        }
                        default: ;
                    }
                }
            }
        }
        return s_instance->m_exit_code;
    }

    bool WindowSystem::is_exited() {
        return s_instance->m_exited.load();
    }

    void WindowSystem::exit(int code) {
        s_instance->m_exited = true;
        s_instance->m_exit_code = code;
    }

    Rc<Window> WindowBuilder::build() const {
        return Window::create(options);
    }

    WindowBuilder Window::builder() {
        return WindowBuilder();
    }

    Rc<Window> Window::create(const WindowOptions &options) {
        const auto sw = SDL_CreateWindow(
            options.title, options.size.x, options.size.y,
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
        Rc win(new Window());
        win->m_inner = std::make_shared<WindowHandle>(sw);
        const auto hwnd = win->hwnd();
        SetMica(hwnd);
        const auto id = win->m_inner->id();
        s_instance->m_windows[id] = win;

        return win;
    }

    const std::shared_ptr<RenderContext> &Window::render_context() {
        if (m_render_context == nullptr) {
            m_render_context = RenderContext::create(*this);
        }
        return m_render_context;
    }

    const std::shared_ptr<WindowHandle> &Window::inner() const {
        return m_inner;
    }

    HWND Window::hwnd() const {
        return m_inner->hwnd();
    }

    int2 Window::size() const {
        return m_inner->size();
    }

    bool Window::resized() const {
        return m_resized;
    }

    void Window::use_resize() {
        m_resized = false;
    }

    void Window::set_gc_handle(void *handle) {
        m_inner->set_gc_handle(handle);
    }

    void *Window::gc_handle() const {
        return m_inner->gc_handle();
    }
} // ccc
