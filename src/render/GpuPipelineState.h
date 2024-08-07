#pragma once
#include "GpuDevice.h"
#include "GpuPipelineLayout.h"
#include "../ffi/gpu/FGpuPipelineState.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuPipelineState final : public FGpuPipelineState
    {
        IMPL_RC(GpuPipelineState);

        Rc<GpuDevice> m_device;
        Rc<GpuPipelineLayout> m_layout;
        com_ptr<ID3D12PipelineState> m_pipeline_state;

        explicit GpuPipelineState(
            Rc<GpuDevice> device, Rc<GpuPipelineLayout> layout, com_ptr<ID3D12PipelineState> pipeline_state
        );

    public:
        static Rc<GpuPipelineState> Create(
            Rc<GpuDevice> device, Rc<GpuPipelineLayout> layout, const FGpuPipelineStateCreateOptions& options,
            FError& err
        ) noexcept;
    };
} // ccc
