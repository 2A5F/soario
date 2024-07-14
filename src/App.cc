#include "App.h"

#include "pch.h"
#include "utils/utils.h"

namespace ccc {
    void App::update() {
    }

    void App::render(const FrameContext &ctx) {
        ctx.cmd.set_rt(ctx.surface);
        ctx.cmd.clear(ctx.surface, float4(1, 1, 1, 1));
    }
} // ccc
