#include "App.h"

#include "pch.h"
#include "utils/utils.h"
#include "time.h"
#include "utils/dotnet.h"
#include "utils/Time.h"

namespace ccc {
    void App::sync_load() {
        load_dotnet();
    }

    void App::update() {
        // spdlog::debug(fmt::format("{}, fps {}", time::delta(), 1.0 / time::delta()));
    }

    void App::render(const FrameContext &ctx) {
        ctx.cmd.set_rt(ctx.surface);
        ctx.cmd.clear(ctx.surface, float4(1, 1, 1, 1));
    }
} // ccc
