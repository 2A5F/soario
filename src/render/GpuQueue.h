#pragma once
#include <directx/d3d12.h>
#include <winrt/base.h>

#include "D3D12MemAlloc.h"
#include "GpuDevice.h"

#include "../pch.h"
#include "../ffi/gpu/FGpu.h"
#include "../ffi/gpu/FGpuQueue.h"
#include "../utils/Rc.h"

namespace ccc
{
    struct FrameContext;
    class GpuSurfaceHwnd;

    class GpuQueue final : public FGpuQueue
    {
        IMPL_RC(GpuQueue);

        friend class GpuSurfaceHwnd;

        static constexpr UINT FrameCount = FGpuConsts::FrameCount;

        Rc<GpuDevice> m_device;

        com_ptr<ID3D12Device> m_dx_device{};
        com_ptr<ID3D12CommandAllocator> m_command_allocators[FrameCount]{};
        com_ptr<ID3D12CommandQueue> m_command_queue{};
        com_ptr<ID3D12GraphicsCommandList> m_command_list{};

        friend FrameContext;

    public:
        explicit GpuQueue(
            Rc<GpuDevice> gpu_device,
            const FGpuQueueCreateOptions& options
        );

        static Rc<GpuQueue> Create(Rc<GpuDevice> gpu_device, const FGpuQueueCreateOptions& options, FError& err) noexcept;

    };
} // ccc
