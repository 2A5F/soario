#include "GpuBuffer.h"

#include "D3D12MemAlloc.h"

#include "./GpuDevice.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuBuffer::GpuBuffer(
        Rc<GpuDevice> device, const FGpuResourceBufferCreateOptions& options, FError& err
    ) : GpuResource(
        std::move(device)
    )
    {
        FGpuResourceBufferInfo info = {};
        info.dimension = FGpuResourceDimension::Buffer;
        info.align = D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
        info.size = options.size;
        info.flags = options.flags;
        m_info.buffer_info = info;

        D3D12MA::ALLOCATION_DESC alloc_desc = {};
        alloc_desc.HeapType = to_dx(options.usage);
        D3D12_RESOURCE_DESC resource_desc = {};
        resource_desc.Dimension = D3D12_RESOURCE_DIMENSION_BUFFER;
        resource_desc.Alignment = D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
        resource_desc.Width = options.size;
        resource_desc.Height = 1;
        resource_desc.DepthOrArraySize = 1;
        resource_desc.MipLevels = 1;
        resource_desc.SampleDesc.Count = 1;
        resource_desc.SampleDesc.Quality = 0;
        resource_desc.Format = DXGI_FORMAT_UNKNOWN;
        resource_desc.Layout = D3D12_TEXTURE_LAYOUT_ROW_MAJOR;
        resource_desc.Flags = to_dx(options.flags);
        D3D12_RESOURCE_STATES states = D3D12_RESOURCE_STATE_COMMON;
        if (options.usage == FGpuResourceUsage::CpuToGpu) states = D3D12_RESOURCE_STATE_GENERIC_READ;
        if (options.usage == FGpuResourceUsage::GpuToCpu) states = D3D12_RESOURCE_STATE_COPY_DEST;

        winrt::check_hresult(
            m_device->m_gpu_allocator->CreateResource(
                &alloc_desc, &resource_desc, states, nullptr, m_allocation.put(), __uuidof(ID3D12Resource), nullptr
            )
        );

        if (options.name.ptr != nullptr)
        {
            winrt::check_hresult(
                m_allocation->GetResource()->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr))
            );
            m_allocation->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr));
        }
    }

    Rc<GpuBuffer> GpuBuffer::Create(
        Rc<GpuDevice> device, const FGpuResourceBufferCreateOptions& options, FError& err
    ) noexcept
    {
        try
        {
            return Rc(new GpuBuffer(std::move(device), options, err));
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to create buffer!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }
} // ccc
