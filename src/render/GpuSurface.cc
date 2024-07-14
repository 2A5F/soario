#include "GpuSurface.h"

#include "GpuCommandList.h"
#include "../window/Window.h"
#include "directx/d3dx12.h"

namespace ccc {
    GpuSurface::GpuSurface(
        const com_ptr<IDXGIFactory4> &factory, com_ptr<ID3D12Device> device,
        const com_ptr<ID3D12CommandQueue> &command_queue, const Window &window
    ) : m_window(window.inner()), m_device(std::move(device)) {
        m_state = GpuRtState::Present;

        const auto size = window.size();

        DXGI_SWAP_CHAIN_DESC1 swap_chain_desc = {};
        swap_chain_desc.BufferCount = FrameCount;
        swap_chain_desc.Width = size.x;
        swap_chain_desc.Height = size.y;
        swap_chain_desc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
        swap_chain_desc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
        swap_chain_desc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD;
        swap_chain_desc.SampleDesc.Count = 1;

        com_ptr<IDXGISwapChain1> swap_chain;
        winrt::check_hresult(factory->CreateSwapChainForHwnd(
            command_queue.get(), // Swap chain needs the queue so that it can force a flush on it.
            window.hwnd(),
            &swap_chain_desc,
            nullptr,
            nullptr,
            swap_chain.put()
        ));
        swap_chain.as(m_swap_chain);

        m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

        /* 创建交换链RTV描述符堆 */
        {
            D3D12_DESCRIPTOR_HEAP_DESC rtv_heap_desc = {};
            rtv_heap_desc.NumDescriptors = FrameCount;
            rtv_heap_desc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
            rtv_heap_desc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
            winrt::check_hresult(m_device->CreateDescriptorHeap(&rtv_heap_desc, RT_IID_PPV_ARGS(m_rtv_heap)));
            m_rtv_descriptor_size = m_device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
        }

        /* 创建帧缓冲区 */
        {
            CD3DX12_CPU_DESCRIPTOR_HANDLE rtvHandle(m_rtv_heap->GetCPUDescriptorHandleForHeapStart());

            // 为每一帧创建一个 RTV。
            for (UINT n = 0; n < FrameCount; n++) {
                winrt::check_hresult(m_swap_chain->GetBuffer(n, RT_IID_PPV_ARGS(m_render_targets[n])));
                m_device->CreateRenderTargetView(m_render_targets[n].get(), nullptr, rtvHandle);
                rtvHandle.Offset(1, m_rtv_descriptor_size);
            }
        }

        /*创建 fence */
        {
            winrt::check_hresult(m_device->CreateFence(0, D3D12_FENCE_FLAG_NONE, RT_IID_PPV_ARGS(m_fence)));
            m_fence_value = 1;

            m_fence_event = CreateEvent(nullptr, FALSE, FALSE, nullptr);
            if (m_fence_event == nullptr) {
                winrt::throw_last_error();
            }
        }
    }

    void GpuSurface::wait_frame_when_drop(const com_ptr<ID3D12CommandQueue> &command_queue) {
        const auto fence = m_fence_value;
        winrt::check_hresult(command_queue->Signal(m_fence.get(), fence));

        while (m_fence->GetCompletedValue() < fence) {
            if (WindowSystem::is_exited()) throw std::exception("exited");
            winrt::check_hresult(m_fence->SetEventOnCompletion(fence, m_fence_event));
            WaitForSingleObject(m_fence_event, 100);
        }
    }

    void GpuSurface::wait_frame(const com_ptr<ID3D12CommandQueue> &command_queue) {
        const auto fence = m_fence_value;
        winrt::check_hresult(command_queue->Signal(m_fence.get(), fence));
        m_fence_value++;

        while (m_fence->GetCompletedValue() < fence) {
            if (WindowSystem::is_exited()) throw std::exception("exited");
            winrt::check_hresult(m_fence->SetEventOnCompletion(fence, m_fence_event));
            WaitForSingleObject(m_fence_event, 100);
        }

        m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

        m_current_cpu_handle = CD3DX12_CPU_DESCRIPTOR_HANDLE(
            m_rtv_heap->GetCPUDescriptorHandleForHeapStart(), m_frame_index, m_rtv_descriptor_size);
    }

    void GpuSurface::present() {
        winrt::check_hresult(m_swap_chain->Present(1, 0));
    }

    CD3DX12_CPU_DESCRIPTOR_HANDLE GpuSurface::get_cpu_handle() {
        return m_current_cpu_handle;
    }

    bool GpuSurface::require_state(GpuRtState target_state, CD3DX12_RESOURCE_BARRIER &barrier) {
        assert_same_thread();
        if (target_state == m_state) return false;
        const auto before = to_dx_state(m_state);
        const auto after = to_dx_state(target_state);
        m_state = target_state;
        barrier = CD3DX12_RESOURCE_BARRIER::Transition(m_render_targets[m_frame_index].get(), before, after);
        return true;
    }
} // ccc
