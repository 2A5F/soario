#include "pch.h"
#include <windows.h>

#include "Args.h"
#include "args.hxx"

#include "utils/utils.h"
#include "window/Window.h"

#include "spdlog/async.h"
#include "spdlog/sinks/stdout_color_sinks.h"
#include "spdlog/sinks/rotating_file_sink.h"

#include <fmt/chrono.h>

#include "render/RenderContext.h"

extern "C" {
__declspec(dllexport) extern const UINT D3D12SDKVersion = 614;
}

extern "C" {
__declspec(dllexport) extern const char *D3D12SDKPath = ".\\D3D12\\";
}

_Use_decl_annotations_

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd) {
    RedirectIOToConsole();

    winrt::init_apartment();

    args::ArgumentParser arg_parser("Soario");
    args::HelpFlag arg_help(arg_parser, "help", "Display this help menu", {'h', "help"});
    args::Flag arg_debug(arg_parser, "debug", "Enable debug mode", {'D', "debug"});
    try {
        arg_parser.ParseCLI(__argc, __argv);
    } catch (const args::Help &) {
        std::cout << arg_parser;
        return 0;
    } catch (const args::ParseError &e) {
        std::cerr << e.what() << std::endl;
        std::cerr << arg_parser;
        return 1;
    } catch (args::ValidationError e) {
        std::cerr << e.what() << std::endl;
        std::cerr << arg_parser;
        return 1;
    }

    /* 初始化日志 */
    auto now = std::chrono::current_zone()->to_local(std::chrono::system_clock::now());
    spdlog::init_thread_pool(8192, 1);
    auto stdout_sink = std::make_shared<spdlog::sinks::stdout_color_sink_mt>();
    auto rotating_sink = std::make_shared<spdlog::sinks::rotating_file_sink_mt>(
        fmt::format("logs/{:%Y.%m.%d}.log", now), 1024 * 1024 * 10, 3);
    std::vector<spdlog::sink_ptr> sinks{stdout_sink, rotating_sink};
    auto logger = std::make_shared<spdlog::async_logger>(
        "", sinks.begin(), sinks.end(), spdlog::thread_pool(), spdlog::async_overflow_policy::block);
    set_default_logger(logger);

    ccc::Args::set(ccc::Args{.debug = arg_debug});

    spdlog::info("start");

    if (arg_debug) spdlog::warn("Debug mode enabled");

    int r;

    try {
        ccc::WindowSystem::init();

        auto window = ccc::Window::builder()
            .title("Soario")
            .size(1280, 720)
            .min_size(640, 360)
            .build();

        std::thread([window] {
            try {
                const auto rdctx = window->render_context();
                while (!ccc::WindowSystem::is_exited()) {
                    rdctx->record_frame([](const ccc::FrameContext &ctx) {
                        ctx.cmd.set_rt(ctx.surface);
                        ctx.cmd.clear(ctx.surface, float4(1, 1, 1, 1));
                    });
                }
            } catch (std::exception ex) {
                spdlog::error(ex.what());
            } catch (...) {
                spdlog::error("Unknown failure occurred. Possible memory corruption");
            }
            SDL_Event event{
                .quit = {
                    .type = SDL_EVENT_QUIT,
                    .timestamp = SDL_GetTicksNS(),
                }
            };
            SDL_PushEvent(&event);
        }).detach();

        r = ccc::WindowSystem::main_loop();
    } catch (std::exception ex) {
        spdlog::error(ex.what());
        r = -1;
    } catch (...) {
        spdlog::error("Unknown failure occurred. Possible memory corruption");
        r = -1;
    }

    SDL_Quit();

    spdlog::info("exited");
    logger->flush();
    return r;
}
