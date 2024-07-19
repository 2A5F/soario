#pragma once
#include "GpuCommandList.h"
#include "GpuQueue.h"

namespace ccc
{
    class RenderContext;

    struct FrameContext final
    {
        RenderContext& render_context;
        GpuQueue& queue;
        GpuCommandList& cmd;
        const std::shared_ptr<GpuSurface>& surface;
    };
} // ccc
