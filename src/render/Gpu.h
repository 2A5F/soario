#pragma once

#include <memory>

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include "directx/d3d12.h"

#include <dxcapi.h>
#include "directx/d3d12shader.h"

#include "D3D12MemAlloc.h"

#include "../pch.h"
#include "../ffi/gpu/FGpu.h"
#include "../utils/Rc.h"
#include "GpuQueue.h"

namespace ccc
{
    class GpuDevice;
    class GpuSurfaceHwnd;

    class Gpu final : public FGpu
    {
        IMPL_RC(Gpu)

    private:
        friend class GpuDevice;
        friend class GpuSurfaceHwnd;

        com_ptr<ID3D12Debug> m_debug_controller{};
        com_ptr<ID3D12InfoQueue1> m_info_queue{};
        DWORD m_callback_cookie{};

        com_ptr<IDXGIFactory4> m_factory{};
        com_ptr<IDXGIAdapter1> m_adapter{};

    public:
        ~Gpu() override;

        explicit Gpu();

        // 获取全局默认的 Gpu 实例
        static void set_global(Rc<Gpu> gpu);

        // 获取全局默认的 Gpu 实例
        static const Rc<Gpu>& global();

        FGpuDevice* CreateDevice(const FGpuDeviceCreateOptions& options, FError& err) noexcept override;
    };
} // ccc
