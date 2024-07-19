#include "Gpu.h"

#include <dxgi1_4.h>
#include <dxgi1_6.h>

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

        void debug_callback(
            D3D12_MESSAGE_CATEGORY Category,
            D3D12_MESSAGE_SEVERITY Severity,
            D3D12_MESSAGE_ID ID,
            LPCSTR pDescription,
            void* pContext
        )
        {
            if (Severity <= D3D12_MESSAGE_SEVERITY_ERROR)
            {
                spdlog::error(fmt::format("[DirectX] {}", pDescription));
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_WARNING)
            {
                spdlog::warn(fmt::format("[DirectX] {}", pDescription));
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_INFO)
            {
                spdlog::info(fmt::format("[DirectX] {}", pDescription));
            }
            else
            {
                spdlog::debug(fmt::format("[DirectX] {}", pDescription));
            }
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

        winrt::check_hresult(D3D12CreateDevice(m_adapter.get(), D3D_FEATURE_LEVEL_12_2, RT_IID_PPV_ARGS(m_device)));

        if (args.debug)
        {
            if (SUCCEEDED(m_device -> QueryInterface(RT_IID_PPV_ARGS(m_info_queue))))
            {
                if (!SUCCEEDED(
                    m_info_queue->RegisterMessageCallback(
                        debug_callback, D3D12_MESSAGE_CALLBACK_FLAG_NONE, nullptr, & m_callback_cookie)
                ))
                {
                    spdlog::warn("register message callback failed");
                }
            }
        }

        D3D12_FEATURE_DATA_SHADER_MODEL shader_model = {D3D_SHADER_MODEL_6_6};
        if (FAILED(m_device->CheckFeatureSupport(D3D12_FEATURE_SHADER_MODEL, &shader_model, sizeof(shader_model)))
            || (shader_model.HighestShaderModel < D3D_SHADER_MODEL_6_6))
        {
            throw std::exception("Shader Model 6.6 is not supported");
        }

        D3D12_FEATURE_DATA_D3D12_OPTIONS7 features = {};
        if (FAILED(m_device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS7, &features, sizeof(features)))
            || (features.MeshShaderTier == D3D12_MESH_SHADER_TIER_NOT_SUPPORTED))
        {
            throw std::exception("Mesh Shaders aren't supported!");
        }

        winrt::check_hresult(DxcCreateInstance(CLSID_DxcUtils, RT_IID_PPV_ARGS(m_dxc_utils)));

        /* 创建队列 */
        {
            m_queue_direct = std::make_shared<GpuQueue>(
                m_device, D3D12_COMMAND_LIST_TYPE_DIRECT
            );

            m_queue_compute = std::make_shared<GpuQueue>(
                m_device, D3D12_COMMAND_LIST_TYPE_COMPUTE
            );

            m_queue_copy = std::make_shared<GpuQueue>(
                m_device, D3D12_COMMAND_LIST_TYPE_COPY
            );
        }

        /* 创建分配器 */
        {
            D3D12MA::ALLOCATOR_DESC allocator_desc = {};
            allocator_desc.pDevice = m_device.get();
            allocator_desc.pAdapter = m_adapter.get();
            allocator_desc.Flags =
                D3D12MA::ALLOCATOR_FLAG_MSAA_TEXTURES_ALWAYS_COMMITTED |
                D3D12MA::ALLOCATOR_FLAG_DEFAULT_POOLS_NOT_ZEROED;
            winrt::check_hresult(CreateAllocator(&allocator_desc, m_gpu_allocator.put()));
        }
    }

    void Gpu::set_global(Rc<Gpu> gpu)
    {
        s_gpu = std::move(gpu);
    }

    const Rc<Gpu>& Gpu::global()
    {
        return s_gpu;
    }
} // ccc
