#pragma once
#include <directx/d3d12.h>

#include "D3D12MemAlloc.h"

#include "../pch.h"
#include "../ffi/gpu/FGpuDevice.h"
#include "../utils/Rc.h"

namespace ccc
{
    class Gpu;
    class GpuQueue;
    class GpuSurfaceHwnd;
    class GpuBindLessPipelineLayout;
    class GpuPipelineState;
    class GpuTask;

    class GpuDevice final : public FGpuDevice
    {
        IMPL_RC(GpuDevice);

        friend GpuQueue;
        friend GpuSurfaceHwnd;
        friend GpuBindLessPipelineLayout;
        friend GpuPipelineState;
        friend GpuTask;

        Rc<Gpu> m_gpu;

        com_ptr<ID3D12Device2> m_device{};

        com_ptr<ID3D12InfoQueue1> m_info_queue{};
        DWORD m_callback_cookie{};

        com_ptr<D3D12MA::Allocator> m_gpu_allocator{};

        fn_func__voidp_FLogLevel_charp__void* m_logger;
        void* m_logger_object;
        fn_func__voidp__void* m_logger_drop_object;

        static void debug_callback(
            D3D12_MESSAGE_CATEGORY Category,
            D3D12_MESSAGE_SEVERITY Severity,
            D3D12_MESSAGE_ID ID,
            LPCSTR pDescription,
            void* pContext
        );

        explicit GpuDevice(Rc<Gpu> gpu, com_ptr<ID3D12Device2> device, const FGpuDeviceCreateOptions& options);

    public:
        ~GpuDevice() override;

        static Rc<GpuDevice> Create(Rc<Gpu> m_gpu, const FGpuDeviceCreateOptions& options, FError& err) noexcept;

        FGpuSurface* CreateSurfaceFromHwnd(
            FGpuQueue* queue, const FGpuSurfaceCreateOptions& options, size_t hwnd, FError& err
        ) noexcept override;

        FGpuQueue* CreateQueue(const FGpuQueueCreateOptions& options, FError& err) noexcept override;

        FGpuPipelineLayout* CreateBindLessPipelineLayout(
            const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
        ) noexcept override;

        FGpuPipelineState* CreatePipelineState(
            FGpuPipelineLayout* layout, const FGpuPipelineStateCreateOptions& options, FError& err
        ) noexcept override;
    };
} // ccc
