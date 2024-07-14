#pragma once

#include "render/RenderContext.h"

namespace ccc {

    class App {
    public:
        void update();

        void render(const ccc::FrameContext &ctx);
    };
} // ccc
