#pragma once
#include <directx/d3d12.h>

#include "../ffi/gpu/FGpuRes.h"

namespace ccc
{
    inline D3D12_RESOURCE_STATES to_dx_state(const FGpuResState state)
    {
        switch (state)
        {
        case FGpuResState::Common:
            return D3D12_RESOURCE_STATE_COMMON;
        case FGpuResState::RenderTarget:
            return D3D12_RESOURCE_STATE_RENDER_TARGET;
        default:
            return D3D12_RESOURCE_STATE_COMMON;
        }
    }
} // ccc
