#include "GpuQueue.h"

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

        winrt::check_hresult(
            m_dx_device->CreateCommandAllocator(
                D3D12_COMMAND_LIST_TYPE_DIRECT, RT_IID_PPV_ARGS(m_command_allocators)
            )
        );

        const D3D12_COMMAND_QUEUE_DESC queue_desc = {
            .Type = type,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };

        winrt::check_hresult(m_dx_device->CreateCommandQueue(&queue_desc, RT_IID_PPV_ARGS(m_command_queue)));

        winrt::check_hresult(
            m_dx_device->CreateCommandList(
                0, D3D12_COMMAND_LIST_TYPE_DIRECT,
                m_command_allocators.get(), nullptr, RT_IID_PPV_ARGS(m_command_list)
            )
        );

        winrt::check_hresult(m_command_list->Close());

        if (options.name.ptr != nullptr)
        {
            winrt::check_hresult(m_command_queue->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
            const auto allocator_name = fmt::format(
                L"{} Allocator", reinterpret_cast<const wchar_t*>(options.name.ptr)
            );
            winrt::check_hresult(m_command_allocators->SetName(allocator_name.c_str()));
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

    void GpuQueue::submit(const FGpuCmdList* cmd_list, FError& err) noexcept
    {
        try
        {
            submit_inner(cmd_list, err);
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to submit command list!");
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
        }
    }

    void submit_BarrierTransition(
        const com_ptr<ID3D12GraphicsCommandList>& command_list, const FGpuCmdBarrierTransition& data
    )
    {
        D3D12_RESOURCE_BARRIER barrier = {};
        barrier.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
        barrier.Transition.pResource = static_cast<ID3D12Resource*>(data.res->get_res_raw_ptr());
        barrier.Transition.Subresource = data.sub_res;
        barrier.Transition.StateBefore = to_dx_state(data.pre_state);
        barrier.Transition.StateAfter = to_dx_state(data.cur_state);
        command_list->ResourceBarrier(1, &barrier);
    }

    void submit_ClearRenderTarget(
        const com_ptr<ID3D12GraphicsCommandList>& command_list, uint8_t* const ptr, const FGpuCmdClearRt& data,
        FError& err
    )
    {
        if (!data.flag.any()) return;
        const auto color = reinterpret_cast<const float*>(&data.color);
        if (data.rect_len > 0)
        {
            if (!(
                (data.flag.color && data.rtv != nullptr && data.rtv->has_rtv()) ||
                ((data.flag.depth || data.flag.stencil) && data.dsv != nullptr && data.dsv->has_dsv())
            ))
                return;
            const auto src = reinterpret_cast<FInt4*>(ptr + sizeof FGpuCmdClearRt);
            const auto rects = static_cast<D3D12_RECT*>(_malloca(data.rect_len));
            for (int i = 0; i < data.rect_len; i++)
            {
                const auto [X, Y, Z, W] = src[i];
                rects[i] = {X, Y, Z, W};
            }
            if (data.flag.color && data.rtv != nullptr && data.rtv->has_rtv())
            {
                D3D12_CPU_DESCRIPTOR_HANDLE cpu_handle{data.rtv->get_cpu_rtv_handle(err)};
                if (err.type != FErrorType::None) return;
                command_list->ClearRenderTargetView(
                    cpu_handle, color, data.rect_len, rects
                );
            }
            if ((data.flag.depth || data.flag.stencil) && data.dsv != nullptr && data.dsv->has_dsv())
            {
                D3D12_CLEAR_FLAGS flags = {};
                if (data.flag.depth)
                {
                    flags |= D3D12_CLEAR_FLAG_DEPTH;
                }
                if (data.flag.stencil)
                {
                    flags |= D3D12_CLEAR_FLAG_STENCIL;
                }
                D3D12_CPU_DESCRIPTOR_HANDLE cpu_handle{data.dsv->get_cpu_dsv_handle(err)};
                if (err.type != FErrorType::None) return;
                command_list->ClearDepthStencilView(
                    cpu_handle, flags, data.depth, data.stencil, data.rect_len, rects
                );
            }
        }
        else
        {
            if (data.flag.color && data.rtv != nullptr && data.rtv->has_rtv())
            {
                D3D12_CPU_DESCRIPTOR_HANDLE cpu_handle{data.rtv->get_cpu_rtv_handle(err)};
                if (err.type != FErrorType::None) return;
                command_list->ClearRenderTargetView(
                    cpu_handle, color, 0, nullptr
                );
            }
            if ((data.flag.depth || data.flag.stencil) && data.dsv != nullptr && data.dsv->has_dsv())
            {
                D3D12_CLEAR_FLAGS flags = {};
                if (data.flag.depth)
                {
                    flags |= D3D12_CLEAR_FLAG_DEPTH;
                }
                if (data.flag.stencil)
                {
                    flags |= D3D12_CLEAR_FLAG_STENCIL;
                }
                D3D12_CPU_DESCRIPTOR_HANDLE cpu_handle{data.dsv->get_cpu_dsv_handle(err)};
                if (err.type != FErrorType::None) return;
                command_list->ClearDepthStencilView(
                    cpu_handle, flags, data.depth, data.stencil, 0, nullptr
                );
            }
        }
    }

    void GpuQueue::submit_inner(const FGpuCmdList* cmd_list, FError& err)
    {
        winrt::check_hresult(m_command_list->Reset(m_command_allocators.get(), nullptr));
        for (int i = 0; i < cmd_list->len; ++i)
        {
            const auto ptr = &cmd_list->datas[cmd_list->indexes[i]];
            switch (*reinterpret_cast<FGpuCmdType*>(ptr))
            {
            case FGpuCmdType::BarrierTransition:
                submit_BarrierTransition(m_command_list, *reinterpret_cast<FGpuCmdBarrierTransition*>(ptr));
                break;
            case FGpuCmdType::ClearRt:
                submit_ClearRenderTarget(m_command_list, ptr, *reinterpret_cast<FGpuCmdClearRt*>(ptr), err);
                if (err.type != FErrorType::None) return;
                break;
            default:
                err = make_error(FErrorType::Gpu, u"Unknown command type");
                return;
            }
        }
        winrt::check_hresult(m_command_list->Close());
        ID3D12CommandList* command_lists[] = {m_command_list.get()};
        m_command_queue->ExecuteCommandLists(1, command_lists);
    }
} // ccc
