#include <mimalloc.h>

#include "pch.h"
#include <windows.h>

#include <stacktrace>

#include "utils/utils.h"
#include "window/Window.h"

#include "spdlog/sinks/stdout_color_sinks.h"
#include "spdlog/sinks/rotating_file_sink.h"

#include <fmt/chrono.h>

#include "App.h"

#include <winrt/Windows.UI.Core.h>

#include "render/Gpu.h"
#include "utils/dotnet.h"
#include "utils/logger.h"

extern "C" {
    __declspec(dllexport) extern const UINT D3D12SDKVersion = 614;
}

extern "C" {
    __declspec(dllexport) extern const char* D3D12SDKPath = ".\\D3D12\\";
}

_Use_decl_annotations_

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
{
    RedirectIOToConsole();

    winrt::init_apartment();

    mi_version();

    int r;

    ccc::WindowSystem::init();

    ccc::InitParams init_params{};
    init_params.p_vas = &ccc::app_vars();
    ccc::InitResult init_result;
    load_dotnet(init_params, init_result);
    ccc::app_fn_vtb() = init_result.fn_vtb;

    try
    {
        std::thread(
            []
            {
                ccc::app_fn_vtb().start();

                SDL_Event event{
                    .quit = {
                        .type = SDL_EVENT_QUIT,
                        .timestamp = SDL_GetTicksNS(),
                    }
                };
                SDL_PushEvent(&event);
            }
        ).detach();

        r = ccc::WindowSystem::main_loop();
    }
    catch (std::exception ex)
    {
        ccc::logger::error(ex.what());
        SDL_ShowSimpleMessageBox(SDL_MESSAGEBOX_ERROR, "Error", ex.what(), nullptr);
        r = -1;
    } catch (winrt::hresult_error ex)
    {
        ccc::logger::error(ex.message());
        MessageBox(nullptr, ex.message().c_str(), nullptr, MB_OK);
        r = -1;
    } catch (...)
    {
        ccc::logger::error("Unknown failure occurred. Possible memory corruption");
        SDL_ShowSimpleMessageBox(
            SDL_MESSAGEBOX_ERROR, "Error",
            "Unknown failure occurred. Possible memory corruption", nullptr
        );
        r = -1;
    }

    SDL_Quit();

    ccc::app_fn_vtb().exit();

    return r;
}
