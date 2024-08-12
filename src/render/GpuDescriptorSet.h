#pragma once
#include <directx/d3d12.h>

#include "D3D12MemAlloc.h"

#include "../pch.h"
#include "../ffi/FFI.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuDevice;
    class GpuTask;

    class GpuDescriptorSet final : public FObject
    {
        IMPL_RC(GpuDescriptorSet);

        friend class GpuDevice;
        friend class GpuTask;

        com_ptr<ID3D12Device2> m_device{};

        com_ptr<ID3D12DescriptorHeap> m_descriptor_heap{};

        size_t m_len{0};
        size_t m_cap{1024};

        explicit GpuDescriptorSet(com_ptr<ID3D12Device2> device, D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name, const wchar_t* name);

    public:
        static Rc<GpuDescriptorSet> Create(com_ptr<ID3D12Device2> device, D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name, const wchar_t* name);
    };
} // ccc
