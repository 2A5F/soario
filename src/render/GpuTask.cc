#include "GpuTask.h"

#include "GpuDescriptorSet.h"
#include "GpuQueue.h"
#include "State.h"
#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuTask::GpuTask(
        Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err
    ) : m_device(std::move(device)), m_queue(std::move(queue))
    {
        m_dx_device = m_device->m_device;
        m_command_queue = m_queue->m_command_queue;

        m_fence_pak = GpuFencePak(m_dx_device);

        winrt::check_hresult(
            m_dx_device->CreateCommandAllocator(
                m_queue->m_type, RT_IID_PPV_ARGS(m_command_allocators)
            )
        );
        winrt::check_hresult(
            m_dx_device->CreateCommandList(
                0, m_queue->m_type,
                m_command_allocators.get(), nullptr, RT_IID_PPV_ARGS(m_command_list)
            )
        );

        if (options.name.ptr != nullptr)
        {
            const auto allocator_name = fmt::format(
                L"{} Allocator", reinterpret_cast<const wchar_t*>(options.name.ptr)
            );
            winrt::check_hresult(m_command_allocators->SetName(allocator_name.c_str()));

            const auto fence_name = fmt::format(
                L"{} Fence", reinterpret_cast<const wchar_t*>(options.name.ptr)
            );
            winrt::check_hresult(m_fence_pak.m_fence->SetName(fence_name.c_str()));
        }

        winrt::check_hresult(m_command_list->Close());
        m_state = State::Closed;

        do_reset();
    }

    void GpuTask::do_start()
    {
        // todo Task 持有自己的描述符堆，每次执行脏检测，复制

        ID3D12DescriptorHeap* descriptor_heaps[] = {
            m_device->m_descriptor_list__resources->ready_for_gpu().get(),
            m_device->m_descriptor_list__sampler->ready_for_gpu().get(),
        };
        m_command_list->SetDescriptorHeaps(2, descriptor_heaps);
        m_state = State::Started;
    }

    void GpuTask::start_inner(FError& err)
    {
        if (m_state != State::Closed)
        {
            err = make_error(FErrorType::Gpu, u"Cannot start on an not completed then rested gpu task");
            return;
        }
        do_start();
    }

    void GpuTask::do_end()
    {
        winrt::check_hresult(m_command_list->Close());
        m_state = State::Ended;
        m_fence_pak.signal(m_queue->m_command_queue);
    }

    void submit_BarrierTransition(
        const com_ptr<ID3D12GraphicsCommandList6>& command_list, const FGpuCmdBarrierTransition& data
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
        const com_ptr<ID3D12GraphicsCommandList6>& command_list, uint8_t* const ptr, const FGpuCmdClearRt& data,
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
            const auto rects = static_cast<D3D12_RECT*>(_malloca(data.rect_len * sizeof D3D12_RECT));
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

    void submit_ReadyRasterizer(
        const com_ptr<ID3D12GraphicsCommandList6>& command_list, uint8_t* const ptr, const FGpuCmdReadyRasterizer& data,
        FError& err
    )
    {
        const auto items = reinterpret_cast<FGpuCmdRasterizerInfo*>(ptr + sizeof FGpuCmdReadyRasterizer);
        const auto viewports = static_cast<D3D12_VIEWPORT*>(_malloca(data.len * sizeof D3D12_VIEWPORT));
        const auto rects = static_cast<D3D12_RECT*>(_malloca(data.len * sizeof D3D12_RECT));
        for (int i = 0; i < data.len; i++)
        {
            const auto& item = items[i];
            const auto& sr = item.scissor_rect;
            const auto& vp_r = item.view_port.rect;
            const auto& vp_d = item.view_port.depth_range;
            viewports[i] = {vp_r.X, vp_r.Y, vp_r.Z, vp_r.W, vp_d.X, vp_d.Y};
            rects[i] = {sr.X, sr.Y, sr.Z, sr.W};
        }
        command_list->RSSetViewports(data.len, viewports);
        command_list->RSSetScissorRects(data.len, rects);
    }

    void submit_DispatchMesh(
        const com_ptr<ID3D12GraphicsCommandList6>& command_list, uint8_t* const ptr, const FGpuCmdDispatchMesh& data,
        FError& err
    )
    {
        command_list->SetGraphicsRootSignature(
            static_cast<ID3D12RootSignature*>(data.pipeline->get_layout_ref()->get_raw_ptr())
        );
        command_list->SetPipelineState(static_cast<ID3D12PipelineState*>(data.pipeline->get_raw_ptr()));
        command_list->DispatchMesh(data.thread_groups.X, data.thread_groups.Y, data.thread_groups.Z);
    }

    void GpuTask::submit_inner(const FGpuCmdList* cmd_list, FError& err)
    {
        if (m_state != State::Started)
        {
            err = make_error(FErrorType::Gpu, u"Cannot submit on an unstarted gpu task");
            return;
        }
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
            case FGpuCmdType::ReadyRasterizer:
                submit_ReadyRasterizer(m_command_list, ptr, *reinterpret_cast<FGpuCmdReadyRasterizer*>(ptr), err);
                if (err.type != FErrorType::None) return;
                break;
            case FGpuCmdType::DispatchMesh:
                submit_DispatchMesh(m_command_list, ptr, *reinterpret_cast<FGpuCmdDispatchMesh*>(ptr), err);
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
        winrt::check_hresult(m_command_list->Reset(m_command_allocators.get(), nullptr));
    }

    void GpuTask::wait_reset_inner()
    {
        m_fence_pak.wait();
        do_reset();
    }

    void GpuTask::wait_reset_async_inner(void* obj, fn_action__voidp cb)
    {
        m_fence_pak.wait_async(
            [
                self = Rc<GpuTask>::UnsafeClone(this),
                obj, cb
            ]
            {
                self->do_reset();
                cb(obj);
            }
        );
    }

    void GpuTask::do_reset()
    {
        winrt::check_hresult(m_command_allocators->Reset());
        winrt::check_hresult(m_command_list->Reset(m_command_allocators.get(), nullptr));
        m_state = State::Closed;
    }

    void GpuTask::wait_reset_any_what()
    {
        if (m_state == State::Closed) return;
        if (m_state == State::Ended) wait_reset_inner();
        if (m_state == State::Started)
        {
            do_end();
            wait_reset_inner();
        }
    }

    void GpuTask::ensure_ended()
    {
        if (m_state == State::Started)
        {
            do_end();
        }
    }

    GpuTask::~GpuTask()
    {
        wait_reset_any_what();
    }

    Rc<GpuTask> GpuTask::Create(
        Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err
    ) noexcept
    {
        return ffi_rc_catch(
            err, FErrorType::Gpu, u"Failed to submit command list!", [&]
            {
                Rc r(new GpuTask(std::move(device), std::move(queue), options, err));
                return r;
            }
        );
    }

    void GpuTask::start(FError& err) noexcept
    {
        ffi_void_catch(
            err, FErrorType::Gpu, u"Failed to start gpu task", [&]
            {
                start_inner(err);
            }
        );
    }

    void GpuTask::submit(const FGpuCmdList* cmd_list, FError& err) noexcept
    {
        ffi_void_catch(
            err, FErrorType::Gpu, u"Failed to submit command list!", [&]
            {
                submit_inner(cmd_list, err);
            }
        );
    }

    void GpuTask::end(FError& err) noexcept
    {
        if (m_state != State::Started)
        {
            err = make_error(FErrorType::Gpu, u"Cannot end unstarted gpu task");
            return;
        }
        ffi_void_catch(
            err, FErrorType::Gpu, u"Failed to end gpu task!", [&]
            {
                do_end();
            }
        );
    }

    void GpuTask::wait_reset(FError& err) noexcept
    {
        if (m_state != State::Ended)
        {
            err = make_error(FErrorType::Gpu, u"Cannot wait for completion/reset on an unended task");
            return;
        }
        ffi_void_catch(
            err, FErrorType::Gpu, u"Failed to wait reset!", [&]
            {
                wait_reset_inner();
            }
        );
    }

    void GpuTask::wait_reset_async(FError& err, void* obj, fn_action__voidp cb) noexcept
    {
        if (m_state != State::Ended)
        {
            err = make_error(FErrorType::Gpu, u"Cannot wait for completion/reset on an unended task");
            return;
        }
        ffi_void_catch(
            err, FErrorType::Gpu, u"Failed to wait reset!", [&]
            {
                wait_reset_async_inner(obj, cb);
            }
        );
    }
} // ccc
