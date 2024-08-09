#pragma once

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include <directx/d3dx12_root_signature.h>
#include "directx/d3d12.h"

#include "GpuDevice.h"
#include "GpuFencePak.h"
#include "GpuSurface.h"
#include "../utils/Rc.h"
#include "../utils/String.h"

namespace ccc
{
    class WindowHandle;

    class GpuSurfaceHwnd final : public GpuSurface
    {
        IMPL_RC(GpuSurfaceHwnd);

        Rc<String16> m_name{};

        Rc<Gpu> m_gpu{};
        Rc<GpuDevice> m_device{};
        Rc<GpuQueue> m_queue{};

        com_ptr<ID3D12Device> m_dx_device{};

        com_ptr<IDXGISwapChain3> m_swap_chain{};
        com_ptr<ID3D12DescriptorHeap> m_rtv_heap{};
        UINT m_rtv_descriptor_size{};
        UINT m_frame_index{};
        com_ptr<ID3D12Resource> m_rts[FrameCount]{};

        GpuFencePak m_fences[FrameCount]{};

        CD3DX12_CPU_DESCRIPTOR_HANDLE m_current_cpu_handle{};

        DXGI_FORMAT m_format{DXGI_FORMAT_R8G8B8A8_UNORM};

        bool m_v_sync{true};

        bool m_resized{false};
        int2 m_new_size{};

        int2 m_current_size{};

        explicit GpuSurfaceHwnd(
            Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd,
            int2 size, FError& err
        );

        void create_rts();

        void wait_all_frame() const;

        void move_to_next_frame();

        void on_resize(int2 new_size);

        void present();

        CD3DX12_CPU_DESCRIPTOR_HANDLE get_dx_cpu_handle() const;

    public:
        static Rc<GpuSurfaceHwnd> Create(
            Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd,
            FError& err
        ) noexcept;

        ~GpuSurfaceHwnd() override;

        int32_t frame_count() noexcept override
        {
            return FrameCount;
        }

        FInt2 get_size() const noexcept override;

        void ready_frame(FError& err) noexcept override;

        void present_frame(FError& err) noexcept override;

        void ready_frame();

        bool has_rtv() noexcept override;

        bool has_dsv() noexcept override;

        void resize(FInt2 new_size) noexcept override;

        bool get_v_sync() const noexcept override;

        void set_v_sync(bool v) noexcept override;

        size_t get_cpu_rtv_handle(FError& err) noexcept override;

        size_t get_cpu_dsv_handle(FError& err) noexcept override;

        void* get_res_raw_ptr() noexcept override;
    };
} // ccc
