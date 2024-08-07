#include "GpuQueue.h"
#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuQueue::GpuQueue(
        Rc<GpuDevice> gpu_device,
        const FGpuQueueCreateOptions& options
    ): m_device(std::move(gpu_device))
    {
        m_dx_device = m_device->m_device;

        D3D12_COMMAND_LIST_TYPE type;
        switch (options.type)
        {
        case FGpuQueueType::Compute:
            type = D3D12_COMMAND_LIST_TYPE_COMPUTE;
            break;
        case FGpuQueueType::Copy:
            type = D3D12_COMMAND_LIST_TYPE_COPY;
            break;
        default:
            type = D3D12_COMMAND_LIST_TYPE_DIRECT;
            break;
        }

        for (int i = 0; i < FrameCount; ++i)
        {
            winrt::check_hresult(
                m_dx_device->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE_DIRECT, RT_IID_PPV_ARGS(m_command_allocators[i])
                )
            );
        }

        const D3D12_COMMAND_QUEUE_DESC queue_desc = {
            .Type = type,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };

        winrt::check_hresult(m_dx_device->CreateCommandQueue(&queue_desc, RT_IID_PPV_ARGS(m_command_queue)));

        winrt::check_hresult(
            m_dx_device->CreateCommandList(
                0, D3D12_COMMAND_LIST_TYPE_DIRECT,
                m_command_allocators[0].get(), nullptr, RT_IID_PPV_ARGS(m_command_list)
            )
        );

        winrt::check_hresult(m_command_list->Close());

        if (options.name.ptr != nullptr)
        {
            winrt::check_hresult(m_command_queue->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
            winrt::check_hresult(m_command_list->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
        }
    }

    Rc<GpuQueue> GpuQueue::Create(Rc<GpuDevice> gpu_device, const FGpuQueueCreateOptions& options, FError& err) noexcept
    {
        try
        {
            Rc r(new GpuQueue(std::move(gpu_device), options));
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, "Failed to create queue!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    // void GpuQueue::ready_frame(const int frame_index) const
    // {
    //     winrt::check_hresult(m_command_allocators[frame_index]->Reset());
    //     winrt::check_hresult(m_command_list->Reset(m_command_allocators[frame_index].get(), nullptr));
    // }
} // ccc
