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
#include "GpuSurface.h"

namespace ccc {
    class RenderContext final : public IObject {
        static constexpr UINT FrameCount = 2;

        friend FrameContext;
        friend GpuQueue;

        std::shared_ptr<WindowHandle> m_window{};

        com_ptr<ID3D12Debug> m_debug_controller{};
        com_ptr<ID3D12InfoQueue1> m_info_queue{};
        DWORD m_callback_cookie{};

        com_ptr<IDXGIFactory4> m_factory{};
        com_ptr<IDXGIAdapter1> m_adapter{};
        com_ptr<ID3D12Device> m_device{};

        std::shared_ptr<GpuSurface> m_surface{};

        com_ptr<D3D12MA::Allocator> m_gpu_allocator{};

        std::shared_ptr<GpuQueue> m_queue_direct{};
        std::shared_ptr<GpuQueue> m_queue_compute{};
        std::shared_ptr<GpuQueue> m_queue_copy{};

    public:
        ~RenderContext() override;

        static std::shared_ptr<RenderContext> create(const Window &window);

        // 记录帧
        void record_frame(const std::function<void(const FrameContext &ctx)> &cb);
    };
} // ccc
