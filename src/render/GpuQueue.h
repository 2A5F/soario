#pragma once
#include <directx/d3d12.h>
#include <winrt/base.h>

#include "D3D12MemAlloc.h"

#include "../pch.h"
#include "../utils/IObject.h"

namespace ccc {
    class RenderContext;
    struct FrameContext;

    class GpuQueue final : public IObject {
        com_ptr<ID3D12Device> m_device{};
        com_ptr<D3D12MA::Allocator> m_gpu_allocator{};
        com_ptr<ID3D12CommandAllocator> m_command_allocator;
        com_ptr<ID3D12CommandQueue> m_command_queue;
        com_ptr<ID3D12GraphicsCommandList> m_command_list;

        friend FrameContext;
        friend RenderContext;

    public:
        explicit GpuQueue(
            com_ptr<ID3D12Device> device,
            com_ptr<D3D12MA::Allocator> gpu_allocator,
            com_ptr<ID3D12CommandAllocator> command_allocator,
            com_ptr<ID3D12CommandQueue> command_queue,
            com_ptr<ID3D12GraphicsCommandList> m_command_list
        );
    };
} // ccc
