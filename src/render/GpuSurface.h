#pragma once
#include <memory>

#include <dxgi1_2.h>
#include <dxgi1_4.h>

#include "directx/d3d12.h"

#include "IRT.h"

#include "../pch.h"

namespace ccc
{
    class GpuQueue;
    class RenderContext;
    class WindowHandle;
    class Window;

    class GpuSurface final : public virtual IRT
    {
        friend RenderContext;
        friend GpuQueue;

        static constexpr UINT FrameCount = 2;

        std::shared_ptr<WindowHandle> m_window;

        com_ptr<ID3D12Device> m_device;

        com_ptr<IDXGISwapChain3> m_swap_chain{};
        com_ptr<ID3D12DescriptorHeap> m_rtv_heap{};
        UINT m_rtv_descriptor_size{};
        UINT m_frame_index{};
        com_ptr<ID3D12Resource> m_render_targets[FrameCount]{};

        UINT64 m_fence_values[FrameCount]{};
        com_ptr<ID3D12Fence> m_fences[FrameCount]{};
        HANDLE m_fence_event{};

        CD3DX12_CPU_DESCRIPTOR_HANDLE m_current_cpu_handle{};

        DXGI_FORMAT m_format{DXGI_FORMAT_R8G8B8A8_UNORM};

        bool m_v_sync{true};

        bool m_resized{false};
        int2 m_new_size{};

        int2 m_current_size{};

    private:
        void create_rts();

        void set_v_sync(bool v);

        UINT frame_index() const;

        void wait_gpu(const com_ptr<ID3D12CommandQueue>& command_queue);

        void move_to_next_frame(const com_ptr<ID3D12CommandQueue>& command_queue);

        void on_resize(int2 new_size);

        // 呈现
        void present();

    public:
        explicit GpuSurface(
            size_t resource_owner_id,
            const com_ptr<IDXGIFactory4>& factory, com_ptr<ID3D12Device> device,
            const com_ptr<ID3D12CommandQueue>& command_queue, const Window& window
        );

        int2 size() const override;

    protected:
        CD3DX12_CPU_DESCRIPTOR_HANDLE get_cpu_handle() override;

        bool require_state(
            ResourceOwner& owner, GpuRtState target_state, CD3DX12_RESOURCE_BARRIER& barrier
        ) override;
    };
} // ccc
