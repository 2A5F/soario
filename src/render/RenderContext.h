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
#include "IResourceOwner.h"

namespace ccc {
    class RenderContext final : public virtual IObject {
        static constexpr UINT FrameCount = GpuSurface::FrameCount;

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

        com_ptr<ID3D12CommandAllocator> m_command_allocators[FrameCount];

        std::shared_ptr<GpuQueue> m_queue_direct{};
        std::shared_ptr<GpuQueue> m_queue_compute{};
        std::shared_ptr<GpuQueue> m_queue_copy{};

        std::shared_ptr<IResourceOwner> m_resource_owner = std::make_shared<IResourceOwner>();

    public:
        ~RenderContext() override;

        static void set_global(std::shared_ptr<RenderContext> ctx);

        // 获取全局的默认渲染上下文
        static std::shared_ptr<RenderContext> global();

        static std::shared_ptr<RenderContext> create(const Window &window);

        void on_resize(Window &window) const;

        // 记录帧
        void record_frame(const std::function<void(const FrameContext &ctx)> &cb);
    };
} // ccc
