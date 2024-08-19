#pragma once
#include "GpuResource.h"
#include "../ffi/gpu/FGpuView.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuView final : public FGpuView
    {
        IMPL_RC(GpuView);

        FGpuViewType m_type;
        Rc<GpuResource> m_resource;

        explicit GpuView(Rc<GpuResource> resource, const FGpuViewCreateOptions& options, FError& err);

    public:
        static Rc<GpuView> Create(Rc<GpuResource> resource, const FGpuViewCreateOptions& options, FError& err);

        FGpuViewType type() const noexcept override;
    };
} // ccc
