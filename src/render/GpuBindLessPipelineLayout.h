#pragma once
#include "GpuDevice.h"
#include "GpuPipelineLayout.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuBindLessPipelineLayout : public GpuPipelineLayout
    {
        IMPL_RC(GpuBindLessPipelineLayout);

        Rc<GpuDevice> m_device;
        com_ptr<ID3D12RootSignature> m_root_signature{};

        explicit GpuBindLessPipelineLayout(
            Rc<GpuDevice> device, const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
        );

    public:
        static Rc<GpuBindLessPipelineLayout> Create(
            Rc<GpuDevice> device, const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
        ) noexcept;

        void* get_raw_ptr() const noexcept override;
    };
} // ccc
