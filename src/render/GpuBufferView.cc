#include "GpuBufferView.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

#include "GpuBuffer.h"
#include "GpuDescriptorSet.h"

namespace ccc
{
    GpuBufferView::GpuBufferView(
        Rc<GpuBuffer> buffer, Rc<GpuDescriptorHandle> descriptor_handle, const FGpuViewCreateOptions& options,
        FError& err
    )
        : GpuView(options.type), m_buffer(std::move(buffer)), m_descriptor_handle(std::move(descriptor_handle))
    {
    }

    GpuResource* GpuBufferView::get_resource() const
    {
        return m_buffer.get();
    }

    Rc<GpuBufferView> GpuBufferView::Create(Rc<GpuBuffer> buffer, const FGpuViewCreateOptions& options, FError& err)
    {
        return ffi_rc_catch(
            err, FErrorType::Gpu, u"Failed to create buffer!", [&]()
            {
                if (options.type == FGpuViewType::Rtv || options.type == FGpuViewType::Dsv)
                {
                    err = make_error(FErrorType::Gpu, u"Buffer does not support rtv and dsv");
                    return Rc<GpuBufferView>();
                }
                if (!(options.type == FGpuViewType::Uav || options.type == FGpuViewType::Srv || options.type ==
                    FGpuViewType::Cbv))
                {
                    err = make_error(FErrorType::Gpu, u"Unsupported view type");
                    return Rc<GpuBufferView>();
                }
                const auto& device = buffer->m_device->m_device;
                const auto desc_handle = buffer->m_device->m_descriptor_list__resources->alloc(buffer);
                if (options.type == FGpuViewType::Uav)
                {
                    device->CreateUnorderedAccessView(
                        buffer->get_resource(), nullptr, nullptr, desc_handle->cpu_handle()
                    );
                }
                else if (options.type == FGpuViewType::Srv)
                {
                    device->CreateShaderResourceView(
                        buffer->get_resource(), nullptr, desc_handle->cpu_handle()
                    );
                }
                else if (options.type == FGpuViewType::Cbv)
                {
                    D3D12_CONSTANT_BUFFER_VIEW_DESC desc = {};
                    desc.BufferLocation = buffer->get_resource()->GetGPUVirtualAddress();
                    desc.SizeInBytes = buffer->get_info()->buffer_info.size;
                    device->CreateConstantBufferView(
                        &desc, desc_handle->cpu_handle()
                    );
                }
                return Rc(new GpuBufferView(std::move(buffer), desc_handle, options, err));
            }
        );
    }
} // ccc
