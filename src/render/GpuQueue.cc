#include "GpuQueue.h"

#include "GpuTask.h"
#include "State.h"
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

        switch (options.type)
        {
        case FGpuQueueType::Compute:
            m_type = D3D12_COMMAND_LIST_TYPE_COMPUTE;
            break;
        case FGpuQueueType::Copy:
            m_type = D3D12_COMMAND_LIST_TYPE_COPY;
            break;
        default:
            m_type = D3D12_COMMAND_LIST_TYPE_DIRECT;
            break;
        }

        const D3D12_COMMAND_QUEUE_DESC queue_desc = {
            .Type = m_type,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };

        winrt::check_hresult(m_dx_device->CreateCommandQueue(&queue_desc, RT_IID_PPV_ARGS(m_command_queue)));

        if (options.name.ptr != nullptr)
        {
            winrt::check_hresult(m_command_queue->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
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
            err = make_error(FErrorType::Gpu, u"Failed to create queue!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    FGpuTask* GpuQueue::CreateTask(const FGpuTaskCreateOptions& options, FError& err) noexcept
    {
        auto r = GpuTask::Create(m_device, Rc<GpuQueue>::UnsafeClone(this), options, err);
        return r.leak();
    }
} // ccc
