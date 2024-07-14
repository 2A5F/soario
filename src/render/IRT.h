#pragma once

#include "../pch.h"
#include "../utils/IObject.h"
#include "directx/d3dx12.h"

namespace ccc {
    class GpuCommandList;

    enum class GpuRtState {
        Present,
        RenderTarget,
    };

    D3D12_RESOURCE_STATES to_dx_state(const GpuRtState state);

    class IRT : public ISingleThreadObject {
        friend GpuCommandList;

    protected:
        GpuRtState m_state;

        // 请求新的状态，返回是否需要屏障
        virtual bool require_state(GpuRtState target_state, CD3DX12_RESOURCE_BARRIER &barrier) = 0;

        virtual CD3DX12_CPU_DESCRIPTOR_HANDLE get_cpu_handle() = 0;

    public:
        virtual int2 size() const = 0;
    };
} // ccc
