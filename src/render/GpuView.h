#pragma once
#include "GpuResource.h"
#include "../ffi/gpu/FGpuView.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuView : public FGpuView
    {
    protected:
        FGpuViewType m_type;

        virtual GpuResource* get_resource() const = 0;

        explicit GpuView(FGpuViewType type);

    public:
        FGpuViewType type() const noexcept override;
    };
} // ccc
