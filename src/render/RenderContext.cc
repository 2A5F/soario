#include "RenderContext.h"

#include <dxgi1_4.h>
#include <dxgi1_6.h>
#include <stacktrace>

#include "directx/d3dx12.h"

#include "../Args.h"

namespace ccc
{
    namespace
    {
        std::shared_ptr<RenderContext> s_global;

        bool get_adapter(IDXGIFactory1& factory, com_ptr<IDXGIAdapter1>& adapter);

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

    std::shared_ptr<RenderContext> RenderContext::create(const Window& window)
    {
        auto& args = Args::get();

        UINT dxgi_factory_flags = 0;
        com_ptr<ID3D12Debug> debug_controller{};
        com_ptr<ID3D12InfoQueue1> info_queue{};
        DWORD callback_cookie{};

        if (args.debug)
        {
            if (SUCCEEDED(D3D12GetDebugInterface(RT_IID_PPV_ARGS(debug_controller))))
            {
                debug_controller->EnableDebugLayer();
                dxgi_factory_flags |= DXGI_CREATE_FACTORY_DEBUG;
            }
        }

        com_ptr<IDXGIFactory4> factory{};
        winrt::check_hresult(CreateDXGIFactory2(dxgi_factory_flags, RT_IID_PPV_ARGS(factory)));

        com_ptr<IDXGIAdapter1> adapter{};
        if (!get_adapter(*factory, adapter))
            throw std::exception(
                "Unable to create render context, no graphics device or graphics device does not support"
            );

        com_ptr<ID3D12Device> device{};
        winrt::check_hresult(D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, RT_IID_PPV_ARGS(device)));

        if (args.debug)
        {
            if (SUCCEEDED(device -> QueryInterface(RT_IID_PPV_ARGS(info_queue))))
            {
                if (!SUCCEEDED(
                    info_queue->RegisterMessageCallback(
                        debug_callback, D3D12_MESSAGE_CALLBACK_FLAG_NONE, nullptr, & callback_cookie)
                ))
                {
                    spdlog::warn("register message callback failed");
                }
            }
        }

        D3D12_FEATURE_DATA_SHADER_MODEL shader_model = {D3D_SHADER_MODEL_6_6};
        if (FAILED(device->CheckFeatureSupport(D3D12_FEATURE_SHADER_MODEL, &shader_model, sizeof(shader_model)))
            || (shader_model.HighestShaderModel < D3D_SHADER_MODEL_6_6))
        {
            throw std::exception("Shader Model 6.6 is not supported");
        }

        D3D12_FEATURE_DATA_D3D12_OPTIONS7 features = {};
        if (FAILED(device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS7, &features, sizeof(features)))
            || (features.MeshShaderTier == D3D12_MESH_SHADER_TIER_NOT_SUPPORTED))
        {
            throw std::exception("Mesh Shaders aren't supported!");
        }

        auto ctx = std::make_shared<RenderContext>();
        ctx->m_window = window.inner();
        ctx->m_factory = std::move(factory);
        ctx->m_debug_controller = std::move(debug_controller);
        ctx->m_info_queue = std::move(info_queue);
        ctx->m_callback_cookie = callback_cookie;
        ctx->m_adapter = std::move(adapter);
        ctx->m_device = std::move(device);

        /* 创建队列 */
        {
            ctx->m_queue_direct = std::make_shared<GpuQueue>(
                ctx->m_device, D3D12_COMMAND_LIST_TYPE_DIRECT
            );

            ctx->m_queue_compute = std::make_shared<GpuQueue>(
                ctx->m_device, D3D12_COMMAND_LIST_TYPE_COMPUTE
            );

            ctx->m_queue_copy = std::make_shared<GpuQueue>(
                ctx->m_device, D3D12_COMMAND_LIST_TYPE_COPY
            );
        }

        ctx->m_surface = std::make_shared<GpuSurface>(
            ctx->m_resource_owner->resource_owner_id(),
            ctx->m_factory, ctx->m_device, ctx->m_queue_direct->m_command_queue, window
        );

        /* 创建分配器 */
        {
            D3D12MA::ALLOCATOR_DESC allocator_desc = {};
            allocator_desc.pDevice = ctx->m_device.get();
            allocator_desc.pAdapter = ctx->m_adapter.get();
            allocator_desc.Flags =
                D3D12MA::ALLOCATOR_FLAG_MSAA_TEXTURES_ALWAYS_COMMITTED |
                D3D12MA::ALLOCATOR_FLAG_DEFAULT_POOLS_NOT_ZEROED;
            winrt::check_hresult(CreateAllocator(&allocator_desc, ctx->m_gpu_allocator.put()));
        }

        return ctx;
    }

    void RenderContext::on_resize(Window& window) const
    {
        window.use_resize();
        const auto size = window.size();
        m_surface->on_resize(size);
    }

    void RenderContext::set_global(std::shared_ptr<RenderContext> ctx)
    {
        s_global = std::move(ctx);
    }

    std::shared_ptr<RenderContext> RenderContext::global()
    {
        return s_global;
    }

    RenderContext::~RenderContext()
    {
        m_surface->wait_gpu(m_queue_direct->m_command_queue);
        if (m_info_queue.get() != nullptr && m_callback_cookie != 0)
        {
            m_info_queue->UnregisterMessageCallback(m_callback_cookie);
        }
    }

    namespace
    {
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

    void RenderContext::record_frame(const std::function<void(const FrameContext& ctx)>& cb)
    {
        const auto queue = this->m_queue_direct;

        // 等待上一帧
        m_surface->move_to_next_frame(queue->m_command_queue);

        // 记录命令
        queue->ready_frame(m_surface->frame_index());

        const auto list = queue->m_command_list;

        GpuCommandList list_box(m_resource_owner, list);
        cb(FrameContext{*this, *queue, list_box, m_surface});

        if (CD3DX12_RESOURCE_BARRIER barrier; m_surface->require_state(
            *m_resource_owner, GpuRtState::Present, barrier
        ))
        {
            list->ResourceBarrier(1, &barrier);
        }

        winrt::check_hresult(list->Close());

        ID3D12CommandList* lists[] = {list.get()};
        queue->m_command_queue->ExecuteCommandLists(1, lists);

        // 呈现
        m_surface->present();
    }
} // ccc
