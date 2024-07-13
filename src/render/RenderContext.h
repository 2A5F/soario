#pragma once
#include <memory>

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include "directx/d3d12.h"

#include "D3D12MemAlloc.h"
#include "GpuQueue.h"

#include "../pch.h"
#include "../window/Window.h"
#include "FrameContext.h"

namespace ccc {
    class RenderContext final {
        static constexpr UINT FrameCount = 2;

        friend FrameContext;
        friend GpuQueue;

        com_ptr<ID3D12Debug> m_debug_controller{};
        com_ptr<ID3D12InfoQueue1> m_info_queue{};
        DWORD m_callback_cookie{};

        com_ptr<IDXGIAdapter1> m_adapter{};
        com_ptr<ID3D12Device> m_device{};

        com_ptr<IDXGISwapChain3> m_swap_chain{};
        com_ptr<ID3D12DescriptorHeap> m_rtv_heap{};
        UINT m_rtv_descriptor_size{};
        UINT m_frame_index{};
        com_ptr<ID3D12Resource> m_render_targets[FrameCount]{};

        com_ptr<D3D12MA::Allocator> m_gpu_allocator{};

        std::shared_ptr<GpuQueue> m_queue_direct{};
        std::shared_ptr<GpuQueue> m_queue_compute{};
        std::shared_ptr<GpuQueue> m_queue_copy{};

        UINT64 m_fence_value;
        com_ptr<ID3D12Fence> m_fence;
        HANDLE m_fence_event;

    public:
        ~RenderContext();

        static std::shared_ptr<RenderContext> create(const Window &window);

        // 记录帧
        void record_frame(std::function<void(FrameContext ctx)> cb);
    };
} // ccc
