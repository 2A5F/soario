#pragma once
#include <directx/d3d12.h>

#include "../pch.h"

namespace ccc {
    class RenderContext;
}

namespace ccc {
    enum class GpuCommandListRtState {
        None,
        Common,
        Swapchain,
    };

    class GpuCommandList {
        friend RenderContext;

        com_ptr<ID3D12GraphicsCommandList> m_command_list{};

        GpuCommandListRtState m_rt_state;

        explicit GpuCommandList(com_ptr<ID3D12GraphicsCommandList> command_list);

    public:

        // 只清除颜色
        void clear(float4 color);
    };
} // ccc
