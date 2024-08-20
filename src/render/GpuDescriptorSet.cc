#include "GpuDescriptorSet.h"
#include "GpuDevice.h"

#include "directx/d3dx12.h"

namespace ccc
{
    GpuDescriptorHandleData::GpuDescriptorHandleData(Rc<GpuResource> resource, const size_t index) : resource(
        std::move(resource)
    ), index(index)
    {
    }

    GpuDescriptorHandle::GpuDescriptorHandle(const Rc<GpuDescriptorSet>& descriptor_set, GpuDescriptorHandleData* data)
        : m_descriptor_set(std::move(descriptor_set)),
        m_data(data)
    {
    }

    D3D12_CPU_DESCRIPTOR_HANDLE GpuDescriptorHandle::cpu_handle() const
    {
        const CD3DX12_CPU_DESCRIPTOR_HANDLE cpu_start(
            m_descriptor_set->m_cpu_descriptor_heap->GetCPUDescriptorHandleForHeapStart(),
            m_data->index, m_descriptor_set->m_descriptor_size
        );
        return cpu_start;
    }

    GpuDescriptorSet::GpuDescriptorSet(
        com_ptr<ID3D12Device2> device, const D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name,
        const wchar_t* name
    ) :
        m_device(std::move(device)), m_type(type)
    {
        D3D12_DESCRIPTOR_HEAP_DESC desc{};
        desc.Type = m_type;
        desc.NumDescriptors = m_cap + 1;
        winrt::check_hresult(m_device->CreateDescriptorHeap(&desc, RT_IID_PPV_ARGS(m_cpu_descriptor_heap)));

        if (!(m_type == D3D12_DESCRIPTOR_HEAP_TYPE_RTV || m_type == D3D12_DESCRIPTOR_HEAP_TYPE_DSV))
        {
            desc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
            winrt::check_hresult(m_device->CreateDescriptorHeap(&desc, RT_IID_PPV_ARGS(m_gpu_descriptor_heap)));
            has_gpu_heap = true;
        }

        m_descriptor_size = m_device->GetDescriptorHandleIncrementSize(m_type);

        if (base_name != nullptr)
        {
            if (has_gpu_heap)
            {
                {
                    const auto w_name = fmt::format(L"{} {} Cpu Descriptor Set", base_name, name);
                    winrt::check_hresult(m_cpu_descriptor_heap->SetName(w_name.c_str()));
                }
                {
                    const auto w_name = fmt::format(L"{} {} Gpu Descriptor Set", base_name, name);
                    winrt::check_hresult(m_gpu_descriptor_heap->SetName(w_name.c_str()));
                }
            }
            else
            {
                const auto w_name = fmt::format(L"{} {} Descriptor Set", base_name, name);
                winrt::check_hresult(m_cpu_descriptor_heap->SetName(w_name.c_str()));
            }
        }

        m_handles = new GpuDescriptorHandleData*[m_cap];
    }

    GpuDescriptorSet::~GpuDescriptorSet()
    {
        for (auto i = m_len; i > 0; i--)
        {
            m_handles[i - 1]->~GpuDescriptorHandleData();
        }
        delete[] m_handles;
    }

    Rc<GpuDescriptorSet> GpuDescriptorSet::Create(
        com_ptr<ID3D12Device2> device, const D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name,
        const wchar_t* name
    )
    {
        Rc r(new GpuDescriptorSet(std::move(device), type, base_name, name));
        return r;
    }

    void GpuDescriptorSet::mark_cpu_dirty()
    {
        cpu_dirty = true;
    }

    const com_ptr<ID3D12DescriptorHeap>& GpuDescriptorSet::ready_for_gpu()
    {
        if (!has_gpu_heap)
        {
            return m_cpu_descriptor_heap;
        }
        else
        {
            if (cpu_dirty.exchange(false))
            {
                std::lock_guard lk(mutex);
                m_device->CopyDescriptorsSimple(
                    m_cap, m_gpu_descriptor_heap->GetCPUDescriptorHandleForHeapStart(),
                    m_cpu_descriptor_heap->GetCPUDescriptorHandleForHeapStart(), m_type
                );
            }
            return m_gpu_descriptor_heap;
        }
    }

    Rc<GpuDescriptorHandle> GpuDescriptorSet::alloc(Rc<GpuResource> resource)
    {
        std::lock_guard lk(mutex);
        mark_cpu_dirty();
        if (m_len >= m_cap)
        {
            // todo grow
            throw std::exception("todo");
        }
        const auto i = m_len;
        m_len++;
        const auto handle = m_handles[i] = new GpuDescriptorHandleData(std::move(resource), m_len);
        return Rc(new GpuDescriptorHandle(Rc<GpuDescriptorSet>::UnsafeClone(this), handle));
    }

    void GpuDescriptorSet::free(const GpuDescriptorHandleData* handle)
    {
        std::lock_guard lk(mutex);
        mark_cpu_dirty();
        const auto null_cpu_handle = calc_cpu_handle(-1);
        const auto i = handle->index;
        const auto cur_cpu_handle = calc_cpu_handle(i);
        if (m_len == 1)
        {
            m_device->CopyDescriptorsSimple(1, cur_cpu_handle, null_cpu_handle, m_type);
        }
        else
        {
            const auto last = m_len - 1;
            const auto last_cpu_handle = calc_cpu_handle(last);
            m_device->CopyDescriptorsSimple(1, cur_cpu_handle, last_cpu_handle, m_type);
            m_device->CopyDescriptorsSimple(1, last_cpu_handle, null_cpu_handle, m_type);
            m_handles[i] = m_handles[last];
            m_handles[i]->index = i;
        }
        delete handle;
        m_len--;
    }

    D3D12_CPU_DESCRIPTOR_HANDLE GpuDescriptorSet::calc_cpu_handle(const int index) const
    {
        const CD3DX12_CPU_DESCRIPTOR_HANDLE cpu_start(
            m_cpu_descriptor_heap->GetCPUDescriptorHandleForHeapStart(),
            index + 1, m_descriptor_size
        );
        return cpu_start;
    }
} // ccc
