#pragma once
#include "GpuDescriptorSet.h"
#include "GpuView.h"

namespace ccc
{
    class GpuBuffer;

    class GpuBufferView final : public GpuView
    {
        IMPL_RC(GpuBufferView);

        Rc<GpuBuffer> m_buffer{};
        Rc<GpuDescriptorHandle> m_descriptor_handle{};

        explicit GpuBufferView(
            Rc<GpuBuffer> buffer, Rc<GpuDescriptorHandle> descriptor_handle, const FGpuViewCreateOptions& options,
            FError& err
        );

    protected:
        GpuResource* get_resource() const override;

    public:
        static Rc<GpuBufferView> Create(Rc<GpuBuffer> buffer, const FGpuViewCreateOptions& options, FError& err);
    };
} // ccc
