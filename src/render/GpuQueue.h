#pragma once
#include <directx/d3d12.h>
#include <winrt/base.h>

#include "D3D12MemAlloc.h"
#include "GpuSurface.h"

#include "../pch.h"
#include "../utils/Object.h"

namespace ccc
{
    class RenderContext;
    struct FrameContext;

    class GpuQueue final : public virtual Object
    {
        static constexpr UINT FrameCount = GpuSurface::FrameCount;

        com_ptr<ID3D12Device> m_device{};
        com_ptr<ID3D12CommandAllocator> m_command_allocators[FrameCount]{};
        com_ptr<ID3D12CommandQueue> m_command_queue{};
        com_ptr<ID3D12GraphicsCommandList> m_command_list{};

        friend FrameContext;
        friend RenderContext;

    public:
        explicit GpuQueue(
            com_ptr<ID3D12Device> device,
            D3D12_COMMAND_LIST_TYPE type
        );

        void ready_frame(int frame_index) const;
    };
} // ccc
