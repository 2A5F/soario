#include <mimalloc.h>

#include "pch.h"
#include <windows.h>

#include <stacktrace>

#include "Args.h"
#include "args.hxx"

#include "utils/utils.h"
#include "window/Window.h"

#include "spdlog/async.h"
#include "spdlog/sinks/stdout_color_sinks.h"
#include "spdlog/sinks/rotating_file_sink.h"

#include <fmt/chrono.h>

#include "App.h"
#include "utils/Time.h"

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

    args::ArgumentParser arg_parser("Soario");
    args::HelpFlag arg_help(arg_parser, "help", "Display this help menu", {'h', "help"});
    args::Flag arg_debug(arg_parser, "debug", "Enable debug mode", {'D', "debug"});
    try
    {
        arg_parser.ParseCLI(__argc, __argv);
    }
    catch (const args::Help&)
    {
        std::cout << arg_parser;
        return 0;
    } catch (const args::ParseError& e)
    {
        std::cerr << e.what() << std::endl;
        std::cerr << arg_parser;
        return 1;
    } catch (args::ValidationError e)
    {
        std::cerr << e.what() << std::endl;
        std::cerr << arg_parser;
        return 1;
    }

    ccc::Args args;
    args.exe_path = std::string(__argv[0]);
    args.debug = arg_debug;
    ccc::Args::set(args);

    int r;

    try
    {
        ccc::WindowSystem::init();

        ccc::Gpu::set_global(new ccc::Gpu());

        auto gpu = ccc::Gpu::global();

        ccc::time::init();
        ccc::time::tick();
        ccc::InitParams init_params{};
        init_params.p_time_data = ccc::time::get_data_ptr();
        init_params.p_gpu = gpu.leak();
        ccc::InitResult init_result;
        load_dotnet(init_params, init_result);
        ccc::app_fn_vtb() = init_result.fn_vtb;

        if (arg_debug)
        {
            ccc::logger::warn("Debug mode enabled");
            ccc::logger::debug(std::format("exe path: {}", args.exe_path));
        }

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
