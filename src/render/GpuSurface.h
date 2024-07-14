#pragma once
#include <memory>

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include "directx/d3d12.h"

#include "GpuQueue.h"
#include "IRT.h"

#include "../pch.h"

namespace ccc {
    class WindowHandle;
    class Window;

    class GpuSurface final : public IRT {
        friend RenderContext;

        static constexpr UINT FrameCount = 2;

        std::shared_ptr<WindowHandle> m_window;

        com_ptr<ID3D12Device> m_device;

        com_ptr<IDXGISwapChain3> m_swap_chain{};
        com_ptr<ID3D12DescriptorHeap> m_rtv_heap{};
        UINT m_rtv_descriptor_size{};
        UINT m_frame_index{};
        com_ptr<ID3D12Resource> m_render_targets[FrameCount]{};

        UINT64 m_fence_value{};
        com_ptr<ID3D12Fence> m_fence{};
        HANDLE m_fence_event{};

        CD3DX12_CPU_DESCRIPTOR_HANDLE m_current_cpu_handle{};

        // 等待上一帧完成
        void wait_frame_when_drop(const com_ptr<ID3D12CommandQueue> &command_queue);

        // 等待上一帧完成
        void wait_frame(const com_ptr<ID3D12CommandQueue> &command_queue);

        // 呈现
        void present();

    public:
        explicit GpuSurface(
            const com_ptr<IDXGIFactory4> &factory, com_ptr<ID3D12Device> device,
            const com_ptr<ID3D12CommandQueue> &command_queue, const Window &window
        );

    protected:
        CD3DX12_CPU_DESCRIPTOR_HANDLE get_cpu_handle() override;

        bool require_state(GpuRtState target_state, CD3DX12_RESOURCE_BARRIER &barrier) override;
    };
} // ccc
