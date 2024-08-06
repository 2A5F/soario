#include "Gpu.h"

#include <dxgi1_4.h>
#include <dxgi1_6.h>

#include "GpuDevice.h"
#include "directx/d3dx12.h"

#include "../Args.h"

namespace ccc
{
    namespace
    {
        Rc<Gpu> s_gpu;

        bool get_adapter(IDXGIFactory1& factory, com_ptr<IDXGIAdapter1>& adapter)
        {
            com_ptr<IDXGIFactory6> factory6{};
            if (SUCCEEDED(factory .QueryInterface(RT_IID_PPV_ARGS(factory6))))
            {
                for (UINT adapter_index = 0;
                     SUCCEEDED(
                         factory6->EnumAdapterByGpuPreference(
                             adapter_index,
                             DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE,
                             RT_IID_PPV_ARGS(adapter)
                         )
                     );
                     ++adapter_index
                )
                {
                    DXGI_ADAPTER_DESC1 desc;
                    adapter->GetDesc1(&desc);

                    if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE) continue;

                    if (SUCCEEDED(
                        D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, _uuidof(ID3D12Device), nullptr)
                    ))
                    {
                        break;
                    }
                }
            }

            if (adapter.get() == nullptr)
            {
                for (
                    UINT adapter_index = 0;
                    SUCCEEDED(factory.EnumAdapters1(adapter_index, adapter.put()));
                    ++adapter_index
                )
                {
                    DXGI_ADAPTER_DESC1 desc;
                    adapter->GetDesc1(&desc);

                    if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE) continue;

                    if (SUCCEEDED(
                        D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, _uuidof(ID3D12Device), nullptr)
                    ))
                    {
                        break;
                    }
                }
            }

            return adapter.get() != nullptr;
        }
    }

    Gpu::~Gpu()
    {
        if (m_info_queue.get() != nullptr && m_callback_cookie != 0)
        {
            m_info_queue->UnregisterMessageCallback(m_callback_cookie);
        }
    }

    Gpu::Gpu()
    {
        const auto& args = Args::get();

        UINT dxgi_factory_flags = 0;

        if (args.debug)
        {
            if (SUCCEEDED(D3D12GetDebugInterface(RT_IID_PPV_ARGS(m_debug_controller))))
            {
                m_debug_controller->EnableDebugLayer();
                dxgi_factory_flags |= DXGI_CREATE_FACTORY_DEBUG;
            }
        }

        winrt::check_hresult(CreateDXGIFactory2(dxgi_factory_flags, RT_IID_PPV_ARGS(m_factory)));

        if (!get_adapter(*m_factory, m_adapter))
            throw std::exception(
                "Unable to create render context, no graphics device or graphics device does not support"
            );

        // /* 创建队列 */
        // {
        //     m_queue_direct = std::make_shared<GpuQueue>(
        //         m_device, D3D12_COMMAND_LIST_TYPE_DIRECT
        //     );
        //
        //     m_queue_compute = std::make_shared<GpuQueue>(
        //         m_device, D3D12_COMMAND_LIST_TYPE_COMPUTE
        //     );
        //
        //     m_queue_copy = std::make_shared<GpuQueue>(
        //         m_device, D3D12_COMMAND_LIST_TYPE_COPY
        //     );
        // }
    }

    void Gpu::set_global(Rc<Gpu> gpu)
    {
        s_gpu = std::move(gpu);
    }

    const Rc<Gpu>& Gpu::global()
    {
        return s_gpu;
    }

    FGpuDevice* Gpu::CreateDevice(const FGpuDeviceCreateOptions& options, FError& err)
    {
        Rc r = GpuDevice::Create(Rc<Gpu>::UnsafeClone(this), options, err);
        return r.leak();
    }
} // ccc
