#include "GpuDescriptorSet.h"
#include "GpuDevice.h"

namespace ccc
{
    GpuDescriptorSet::GpuDescriptorSet(
        com_ptr<ID3D12Device2> device, const D3D12_DESCRIPTOR_HEAP_TYPE type, const wchar_t* base_name,
        const wchar_t* name
    ) :
        m_device(std::move(device))
    {
        D3D12_DESCRIPTOR_HEAP_DESC desc{};
        desc.Type = type;
        desc.Flags = type == D3D12_DESCRIPTOR_HEAP_TYPE_RTV || type == D3D12_DESCRIPTOR_HEAP_TYPE_DSV
            ? D3D12_DESCRIPTOR_HEAP_FLAG_NONE
            : D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
        desc.NumDescriptors = m_cap;
        winrt::check_hresult(m_device->CreateDescriptorHeap(&desc, RT_IID_PPV_ARGS(m_descriptor_heap)));

        if (base_name != nullptr)
        {
            const auto w_name = fmt::format(L"{} {} Descriptor Set", base_name, name);
            winrt::check_hresult(m_descriptor_heap->SetName(w_name.c_str()));
        }

        m_handles = new GpuDescriptorHandle*[m_cap];
    }

    GpuDescriptorSet::~GpuDescriptorSet()
    {
        for (auto i = m_len; i > 0; i--)
        {
            m_handles[i - 1]->Release();
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

    // void GpuDescriptorSet::Alloc(const Rc<GpuDescriptorHandle>& handle)
    // {
    //     std::lock_guard lk(mutex);
    //     auto i = m_len;
    //     m_len++;
    //     handle->index = m_len;
    //     m_handles[i] = handle.get();
    //     // ReSharper disable once CppExpressionWithoutSideEffects
    //     handle->AddRef();
    //     // todo
    // }
    //
    // void GpuDescriptorSet::Free(const Rc<GpuDescriptorHandle>& handle)
    // {
    //     std::lock_guard lk(mutex);
    //     // todo
    // }
} // ccc
