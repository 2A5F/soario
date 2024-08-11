#include "GpuFencePak.h"

namespace ccc
{
    GpuFencePak::GpuFencePak(const com_ptr<ID3D12Device>& device, const FrStr16 name, int index) : GpuFencePak(device)
    {
        if (name.ptr != nullptr)
        {
            winrt::check_hresult(
                m_fence->SetName(fmt::format(L"{} Fence {}", reinterpret_cast<const wchar_t*>(name.ptr), index).c_str())
            );
        }
    }

    GpuFencePak::GpuFencePak(const com_ptr<ID3D12Device>& device, const FrStr16 name) : GpuFencePak(device)
    {
        if (name.ptr != nullptr)
        {
            winrt::check_hresult(
                m_fence->SetName(fmt::format(L"{} Fence", reinterpret_cast<const wchar_t*>(name.ptr)).c_str())
            );
        }
    }

    GpuFencePak::GpuFencePak(const com_ptr<ID3D12Device>& device)
    {
        winrt::check_hresult(
            device->CreateFence(
                m_fence_value, D3D12_FENCE_FLAG_NONE, RT_IID_PPV_ARGS(m_fence)
            )
        );

        m_fence_event = CreateEvent(nullptr, FALSE, FALSE, nullptr);
        if (m_fence_event == nullptr)
        {
            winrt::throw_last_error();
        }
    }

    void GpuFencePak::wait() const
    {
        const auto& fence = m_fence;
        const auto fence_value = m_fence_value;

        if (fence->GetCompletedValue() < fence_value)
        {
            winrt::check_hresult(fence->SetEventOnCompletion(fence_value, m_fence_event));
            WaitForSingleObjectEx(m_fence_event, INFINITE, false);
        }
    }

    void GpuFencePak::wait_async(std::function<void()> callback) const
    {
        const auto& fence = m_fence;
        const auto fence_value = m_fence_value;

        if (fence->GetCompletedValue() < fence_value)
        {
            winrt::check_hresult(fence->SetEventOnCompletion(fence_value, m_fence_event));
            const auto data = new WaitAsyncData{std::move(callback)};
            HANDLE hNewHandle;
            RegisterWaitForSingleObject(
                &hNewHandle, m_fence_event, [](void* p_data, BOOLEAN timeout)
                {
                    const auto data = static_cast<WaitAsyncData*>(p_data);
                    data->callback();
                    delete data;
                }, data, INFINITE, false
            );
        }
    }

    void GpuFencePak::signal(const com_ptr<ID3D12CommandQueue>& queue)
    {
        const auto& fence = m_fence;
        winrt::check_hresult(queue->Signal(fence.get(), ++m_fence_value));
    }
} // ccc
