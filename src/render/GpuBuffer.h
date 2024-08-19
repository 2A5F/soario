#pragma once
#include <directx/d3d12.h>

#include "GpuDevice.h"
#include "GpuResource.h"
#include "../pch.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuBuffer final : public GpuResource
    {
        IMPL_RC(GpuBuffer);

        explicit GpuBuffer(Rc<GpuDevice> device, const FGpuResourceBufferCreateOptions& options, FError& err);
    public:
        static Rc<GpuBuffer> Create(Rc<GpuDevice> device, const FGpuResourceBufferCreateOptions& options, FError& err) noexcept;
    };
} // ccc
