#include "IWindow.h"

#include "../utils/utf8_utf16.h"
#include "../window/Window.h"
#include "Api.h"
#include "../utils/sdl_error.h"

namespace ccc {
    FWindow *FWindow::create(const WindowCreateOptions &options) {
        auto title = utf16_to_utf8(
            std::wstring_view(reinterpret_cast<const wchar_t *>(options.title.ptr), options.title.len)
        );

        WindowCreateParamPack param_pack{};
        param_pack.title = std::move(title);
        param_pack.size = int2(options.size.X, options.size.Y);
        if (options.has_min_size)
            param_pack.min_size = int2(options.min_size.X, options.min_size.Y);
        param_pack.semaphore = SDL_CreateSemaphore(0);
        if (param_pack.semaphore == nullptr) throw sdl_error();

        SDL_Event event{
            .user = {
                .type = SDL_EVENT_USER,
                .timestamp = SDL_GetTicksNS(),
                .code = static_cast<Sint32>(SoarMsgEvent::CreateWindow),
                .data1 = &param_pack,
            }
        };
        if (SDL_PushEvent(&event) < 0) throw sdl_error();

        const int r = SDL_WaitSemaphore(param_pack.semaphore);
        SDL_DestroySemaphore(param_pack.semaphore);
        if (r != 0) throw sdl_error();

        return param_pack.window.leak();
    }
} // ccc
