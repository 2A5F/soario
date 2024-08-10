#pragma once
#include <directx/d3d12.h>

#include "../ffi/gpu/FGpuPipelineState.h"

namespace ccc
{
    D3D12_BLEND to_dx(FGpuPipelineBlendType type);

    D3D12_BLEND_OP to_dx(FGpuPipelineBlendOp op);

    D3D12_LOGIC_OP to_dx(FGpuPipelineLogicOp op);

    UINT8 to_dx(FGpuPipelineColorMask mask);

    void to_dx(
        D3D12_BLEND_DESC& desc, const FGpuPipelineBlendState& state, const int32_t rt_count
    );

    D3D12_FILL_MODE to_dx(FGpuPipelineFillMode mode);

    D3D12_CULL_MODE to_dx(FGpuPipelineCullMode mode);

    void to_dx(D3D12_RASTERIZER_DESC& desc, const FGpuPipelineRasterizerState& state);

    D3D12_COMPARISON_FUNC to_dx(FGpuPipelineCmpFunc func);

    D3D12_STENCIL_OP to_dx(FGpuPipelineStencilFailOp op);

    void to_dx(D3D12_DEPTH_STENCILOP_DESC& desc, const FGpuPipelineStencilState& state);

    void to_dx(D3D12_DEPTH_STENCIL_DESC& desc, const FGpuPipelineDepthStencilState& state);

    D3D12_PRIMITIVE_TOPOLOGY_TYPE to_dx(FGpuPipelinePrimitiveTopologyType topology);

    DXGI_FORMAT to_dx(FGpuTextureFormat format);
} // ccc
