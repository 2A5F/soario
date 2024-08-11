#pragma once
#include <directx/d3dx12_root_signature.h>
#include "directx/d3d12.h"

#include "../pch.h"
#include "../ffi/FFI.h"
#include "../ffi/FnPtrs.h"

namespace ccc
{
    class GpuFencePak
    {
        UINT64 m_fence_value{};
        com_ptr<ID3D12Fence> m_fence{};
        HANDLE m_fence_event{};

        struct WaitAsyncData
        {
            std::function<void()> callback;
        };

    public:
        GpuFencePak() = default;

        explicit GpuFencePak(const com_ptr<ID3D12Device>& device, FrStr16 name, int index);

        explicit GpuFencePak(const com_ptr<ID3D12Device>& device, FrStr16 name);

        explicit GpuFencePak(const com_ptr<ID3D12Device>& device);

        void wait() const;

        void wait_async(std::function<void()> callback) const;

        void signal(const com_ptr<ID3D12CommandQueue>& queue);

    };
} // ccc
