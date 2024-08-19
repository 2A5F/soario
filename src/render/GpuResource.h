#pragma once
#include <directx/d3d12.h>

#include "D3D12MemAlloc.h"
#include "../ffi/gpu/FGpuResource.h"

#include "../pch.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuDevice;

    D3D12_HEAP_TYPE to_dx(FGpuResourceUsage usage);

    D3D12_RESOURCE_DIMENSION to_dx(FGpuResourceDimension dimension);

    D3D12_RESOURCE_FLAGS to_dx(FGpuResourceFlags flags);

    class GpuResource : public FGpuResource
    {
    protected:
        FGpuResourceInfo m_info{};
        com_ptr<D3D12MA::Allocation> m_allocation{};
        Rc<GpuDevice> m_device{};

        explicit GpuResource(Rc<GpuDevice> device);

    public:
        void* get_res_raw_ptr() noexcept override;

        const FGpuResourceInfo* get_info() const noexcept override;
    };
} // ccc
