#pragma once

#include <memory>

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include "directx/d3d12.h"

#include "D3D12MemAlloc.h"

#include "../pch.h"
#include "../ffi/gpu/FGpu.h"
#include "../utils/Rc.h"
#include "GpuSurface.h"
#include "GpuQueue.h"

namespace ccc
{
    class Gpu final : public FGpu
    {
        IMPL_RC(Gpu)

        static constexpr UINT FrameCount = GpuSurface::FrameCount;

        com_ptr<ID3D12Debug> m_debug_controller{};
        com_ptr<ID3D12InfoQueue1> m_info_queue{};
        DWORD m_callback_cookie{};

        com_ptr<IDXGIFactory4> m_factory{};
        com_ptr<IDXGIAdapter1> m_adapter{};
        com_ptr<ID3D12Device> m_device{};

        com_ptr<D3D12MA::Allocator> m_gpu_allocator{};

        std::shared_ptr<GpuQueue> m_queue_direct{};
        std::shared_ptr<GpuQueue> m_queue_compute{};
        std::shared_ptr<GpuQueue> m_queue_copy{};

        std::shared_ptr<ResourceOwner> m_resource_owner = std::make_shared<ResourceOwner>();

    public:
        ~Gpu() override;

        explicit Gpu();

        // 获取全局默认的 Gpu 实例
        static void set_global(Rc<Gpu> gpu);

        // 获取全局默认的 Gpu 实例
        static const Rc<Gpu>& global();
    };
} // ccc
