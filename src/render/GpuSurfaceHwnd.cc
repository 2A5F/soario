#include "GpuSurfaceHwnd.h"

#include "Gpu.h"
#include "GpuDevice.h"
#include "GpuQueue.h"
#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuSurfaceHwnd::GpuSurfaceHwnd(
        Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd,
        const int2 size, FError& err
    ) : m_device(std::move(device)), m_current_size(size)
    {
        m_gpu = m_device->m_gpu;

        m_dx_device = m_device->m_device;

        DXGI_SWAP_CHAIN_DESC1 swap_chain_desc = {};
        swap_chain_desc.BufferCount = FrameCount;
        swap_chain_desc.Width = size.x;
        swap_chain_desc.Height = size.y;
        swap_chain_desc.Format = m_format;
        swap_chain_desc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
        swap_chain_desc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD;
        swap_chain_desc.SampleDesc.Count = 1;

        com_ptr<IDXGISwapChain1> swap_chain;
        winrt::check_hresult(
            m_gpu->m_factory->CreateSwapChainForHwnd(
                queue->m_command_queue.get(), // Swap chain needs the queue so that it can force a flush on it.
                hwnd,
                &swap_chain_desc,
                nullptr,
                nullptr,
                swap_chain.put()
            )
        );
        swap_chain.as(m_swap_chain);

        m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

        /* 创建交换链RTV描述符堆 */
        {
            D3D12_DESCRIPTOR_HEAP_DESC rtv_heap_desc = {};
            rtv_heap_desc.NumDescriptors = FrameCount;
            rtv_heap_desc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
            rtv_heap_desc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
            winrt::check_hresult(m_dx_device->CreateDescriptorHeap(&rtv_heap_desc, RT_IID_PPV_ARGS(m_rtv_heap)));
            m_rtv_descriptor_size = m_dx_device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
        }

        /* 创建帧缓冲区 */
        create_rts();

        /*创建 fence */
        {
            for (UINT i = 0; i < FrameCount; ++i)
            {
                winrt::check_hresult(
                    m_dx_device->CreateFence(
                        m_fence_values[m_frame_index], D3D12_FENCE_FLAG_NONE, RT_IID_PPV_ARGS(m_fences[i])
                    )
                );
                m_fence_values[m_frame_index]++;
            }

            m_fence_event = CreateEvent(nullptr, FALSE, FALSE, nullptr);
            if (m_fence_event == nullptr)
            {
                winrt::throw_last_error();
            }

            wait_gpu(queue->m_command_queue);
        }
    }

    void GpuSurfaceHwnd::create_rts()
    {
        CD3DX12_CPU_DESCRIPTOR_HANDLE rtvHandle(m_rtv_heap->GetCPUDescriptorHandleForHeapStart());

        // 为每一帧创建一个 RTV。
        for (UINT n = 0; n < FrameCount; n++)
        {
            winrt::check_hresult(m_swap_chain->GetBuffer(n, RT_IID_PPV_ARGS(m_render_targets[n])));
            m_dx_device->CreateRenderTargetView(m_render_targets[n].get(), nullptr, rtvHandle);
            rtvHandle.Offset(1, m_rtv_descriptor_size);
        }
    }

    void GpuSurfaceHwnd::wait_gpu(const com_ptr<ID3D12CommandQueue>& command_queue)
    {
        const auto& fence = m_fences[m_frame_index];
        const auto fence_value = m_fence_values[m_frame_index];
        winrt::check_hresult(command_queue->Signal(fence.get(), fence_value));

        if (fence->GetCompletedValue() < fence_value)
        {
            winrt::check_hresult(fence->SetEventOnCompletion(fence_value, m_fence_event));
            WaitForSingleObjectEx(m_fence_event, INFINITE, false);
        }

        m_fence_values[m_frame_index]++;
    }

    void GpuSurfaceHwnd::move_to_next_frame(const com_ptr<ID3D12CommandQueue>& command_queue)
    {
        if (m_resized && (m_current_size.x != m_new_size.x || m_current_size.y != m_new_size.y))
        {
            m_current_size = m_new_size;

            for (UINT i = 0; i < FrameCount; ++i)
            {
                const auto& fence = m_fences[i];
                const auto fence_value = ++m_fence_values[i];
                winrt::check_hresult(command_queue->Signal(fence.get(), fence_value));
                if (fence->GetCompletedValue() < m_fence_values[i])
                {
                    winrt::check_hresult(fence->SetEventOnCompletion(fence_value, m_fence_event));
                    WaitForSingleObjectEx(m_fence_event, INFINITE, false);
                }
            }

            for (UINT n = 0; n < FrameCount; n++)
            {
                m_render_targets[n] = nullptr;
            }

            DXGI_SWAP_CHAIN_DESC1 desc = {};
            winrt::check_hresult(m_swap_chain->GetDesc1(&desc));
            winrt::check_hresult(
                m_swap_chain->ResizeBuffers(
                    FrameCount, m_new_size.x, m_new_size.y, desc.Format, desc.Flags
                )
            );

            create_rts();

            m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();
        }
        else
        {
            const auto& fence = m_fences[m_frame_index];
            const UINT64 current_fence_value = m_fence_values[m_frame_index];
            winrt::check_hresult(command_queue->Signal(fence.get(), current_fence_value));

            m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

            if (fence->GetCompletedValue() < m_fence_values[m_frame_index])
            {
                winrt::check_hresult(fence->SetEventOnCompletion(m_fence_values[m_frame_index], m_fence_event));
                WaitForSingleObjectEx(m_fence_event, INFINITE, FALSE);
            }

            m_fence_values[m_frame_index] = current_fence_value + 1;
        }

        m_resized = false;
        m_current_cpu_handle = CD3DX12_CPU_DESCRIPTOR_HANDLE(
            m_rtv_heap->GetCPUDescriptorHandleForHeapStart(), m_frame_index, m_rtv_descriptor_size
        );
    }

    void GpuSurfaceHwnd::on_resize(const int2 new_size)
    {
        m_resized = true;
        m_new_size = new_size;
    }

    void GpuSurfaceHwnd::present() const
    {
        winrt::check_hresult(m_swap_chain->Present(m_v_sync ? 1 : 0, 0));
    }

    CD3DX12_CPU_DESCRIPTOR_HANDLE GpuSurfaceHwnd::get_cpu_handle() const
    {
        return m_current_cpu_handle;
    }

    Rc<GpuSurfaceHwnd> GpuSurfaceHwnd::Create(
        Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd, FError& err
    ) noexcept
    {
        int2 size;
        RECT rect;
        if (GetWindowRect(hwnd, &rect))
        {
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            size = {width, height};
        }
        else
        {
            err = make_error(FErrorType::Common, "Failed to get window size");
            return nullptr;
        }

        try
        {
            Rc r(new GpuSurfaceHwnd(std::move(device), queue, options, hwnd, size, err));
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, "Failed to create surface!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    FInt2 GpuSurfaceHwnd::get_size() const noexcept
    {
        return {m_current_size.x, m_current_size.y};
    }

    bool GpuSurfaceHwnd::get_v_sync() const noexcept
    {
        return m_v_sync;
    }

    void GpuSurfaceHwnd::set_v_sync(const bool v) noexcept
    {
        m_v_sync = v;
    }

    // bool GpuSurfaceHwnd::require_state(
    //     ResourceOwner& owner, GpuRtState target_state, CD3DX12_RESOURCE_BARRIER& barrier
    // )
    // {
    //     owner.assert_ownership(this);
    //     if (target_state == m_state) return false;
    //     const auto before = to_dx_state(m_state);
    //     const auto after = to_dx_state(target_state);
    //     m_state = target_state;
    //     barrier = CD3DX12_RESOURCE_BARRIER::Transition(m_render_targets[m_frame_index].get(), before, after);
    //     return true;
    // }
} // ccc
