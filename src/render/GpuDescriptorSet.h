#pragma once
#include <directx/d3d12.h>

#include "D3D12MemAlloc.h"
#include "GpuResource.h"

#include "../pch.h"
#include "../ffi/FFI.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuDevice;
    class GpuTask;

    class GpuDescriptorSet;

    struct GpuDescriptorHandleData final
    {
        Rc<GpuResource> resource{};
        size_t index{};

        explicit GpuDescriptorHandleData(Rc<GpuResource> resource, size_t index);
    };

    class GpuDescriptorHandle final : public FObject
    {
        IMPL_RC(GpuDescriptorHandle);

        friend GpuDescriptorSet;

        Rc<GpuDescriptorSet> m_descriptor_set;
        GpuDescriptorHandleData* m_data;

        GpuDescriptorHandle(const Rc<GpuDescriptorSet>& descriptor_set, GpuDescriptorHandleData* data);

    public:
        D3D12_CPU_DESCRIPTOR_HANDLE cpu_handle() const;
    };

    class GpuDescriptorSet final : public FObject
    {
        IMPL_RC(GpuDescriptorSet);

        friend GpuDevice;
        friend GpuTask;
        friend GpuDescriptorHandle;

        std::recursive_mutex mutex{};

        com_ptr<ID3D12Device2> m_device{};

        D3D12_DESCRIPTOR_HEAP_TYPE m_type{};
        com_ptr<ID3D12DescriptorHeap> m_cpu_descriptor_heap{};
        com_ptr<ID3D12DescriptorHeap> m_gpu_descriptor_heap{};
        size_t m_descriptor_size{};
        bool has_gpu_heap{};
        std::atomic_bool cpu_dirty{};

        GpuDescriptorHandleData** m_handles;

        size_t m_len{0};
        size_t m_cap{1024};

        explicit GpuDescriptorSet(
            com_ptr<ID3D12Device2> device, D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name,
            const wchar_t* name
        );

        void free(const GpuDescriptorHandleData* handle);

        D3D12_CPU_DESCRIPTOR_HANDLE calc_cpu_handle(int index) const;

    public:
        ~GpuDescriptorSet() override;

        static Rc<GpuDescriptorSet> Create(
            com_ptr<ID3D12Device2> device, D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name,
            const wchar_t* name
        );

        const com_ptr<ID3D12DescriptorHeap>& ready_for_gpu();

        Rc<GpuDescriptorHandle> alloc(Rc<GpuResource> resource);

        void mark_cpu_dirty();
    };
} // ccc
