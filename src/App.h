#pragma once

#include "render/RenderContext.h"

namespace ccc {
    class App {
    public:
        // 同步加载，用于无法异步的部分
        void sync_load();

        // 每帧调用的更新事件
        void update();

        // 每帧调用的渲染事件
        void render(const ccc::FrameContext &ctx);
    };
} // ccc
