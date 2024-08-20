#include "GpuResource.h"

#include "GpuDevice.h"

namespace ccc
{
    D3D12_HEAP_TYPE to_dx(const FGpuResourceUsage usage)
    {
        switch (usage)
        {
        case FGpuResourceUsage::CpuToGpu:
            return D3D12_HEAP_TYPE_UPLOAD;
        case FGpuResourceUsage::GpuToCpu:
            return D3D12_HEAP_TYPE_READBACK;
        case FGpuResourceUsage::GpuOnly:
        default:
            return D3D12_HEAP_TYPE_DEFAULT;
        }
    }

    D3D12_RESOURCE_DIMENSION to_dx(const FGpuResourceDimension dimension)
    {
        switch (dimension)
        {
        case FGpuResourceDimension::Buffer:
            return D3D12_RESOURCE_DIMENSION_BUFFER;
        case FGpuResourceDimension::Texture1D:
            return D3D12_RESOURCE_DIMENSION_TEXTURE1D;
        case FGpuResourceDimension::Texture2D:
            return D3D12_RESOURCE_DIMENSION_TEXTURE2D;
        case FGpuResourceDimension::Texture3D:
            return D3D12_RESOURCE_DIMENSION_TEXTURE3D;
        case FGpuResourceDimension::Unknown:
        default:
            return D3D12_RESOURCE_DIMENSION_UNKNOWN;
        }
    }

    D3D12_RESOURCE_FLAGS to_dx(const FGpuResourceFlags flags)
    {
        D3D12_RESOURCE_FLAGS r = D3D12_RESOURCE_FLAG_NONE;
        if (flags.rtv) r |= D3D12_RESOURCE_FLAG_ALLOW_RENDER_TARGET;
        if (flags.dsv) r |= D3D12_RESOURCE_FLAG_ALLOW_DEPTH_STENCIL;
        if (flags.uav) r |= D3D12_RESOURCE_FLAG_ALLOW_UNORDERED_ACCESS;
        if (!flags.srv) r |= D3D12_RESOURCE_FLAG_DENY_SHADER_RESOURCE;
        if (flags.cross_gpu) r |= D3D12_RESOURCE_FLAG_ALLOW_CROSS_ADAPTER;
        if (flags.shared_access) r |= D3D12_RESOURCE_FLAG_ALLOW_SIMULTANEOUS_ACCESS;
        if (flags.ray_tracing_acceleration_structure) r |= D3D12_RESOURCE_FLAG_RAYTRACING_ACCELERATION_STRUCTURE;
        return r;
    }

    GpuResource::GpuResource(Rc<GpuDevice> device) : m_device(std::move(device))
    {
    }

    void* GpuResource::get_res_raw_ptr() noexcept
    {
        return get_resource();
    }

    const FGpuResourceInfo* GpuResource::get_info() const noexcept
    {
        return &m_info;
    }

    ID3D12Resource* GpuResource::get_resource() const
    {
        return m_allocation->GetResource();
    }
} // ccc
