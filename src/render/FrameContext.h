#pragma once
#include "GpuCommandList.h"
#include "GpuQueue.h"

namespace ccc {
    class RenderContext;

    struct FrameContext {
        RenderContext& render_context;
        GpuQueue &queue;
        GpuCommandList& cmd;
    };
} // ccc
