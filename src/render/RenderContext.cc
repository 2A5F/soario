#include "RenderContext.h"

#include <dxgi1_4.h>
#include <dxgi1_6.h>

#include "directx/d3dx12.h"

#include "../Args.h"

namespace ccc {
    namespace {
        bool get_adapter(IDXGIFactory1 &factory, com_ptr<IDXGIAdapter1> &adapter);

        void debug_callback(
            D3D12_MESSAGE_CATEGORY Category,
            D3D12_MESSAGE_SEVERITY Severity,
            D3D12_MESSAGE_ID ID,
            LPCSTR pDescription,
            void *pContext
        ) {
            if (Severity <= D3D12_MESSAGE_SEVERITY_ERROR) {
                spdlog::error(fmt::format("[DirectX] {}", pDescription));
            } else if (Severity == D3D12_MESSAGE_SEVERITY_WARNING) {
                spdlog::warn(fmt::format("[DirectX] {}", pDescription));
            } else if (Severity == D3D12_MESSAGE_SEVERITY_INFO) {
                spdlog::info(fmt::format("[DirectX] {}", pDescription));
            } else {
                spdlog::debug(fmt::format("[DirectX] {}", pDescription));
            }
        }
    }

    std::shared_ptr<RenderContext> RenderContext::create(const Window &window) {
        auto &args = Args::get();

        UINT dxgi_factory_flags = 0;
        com_ptr<ID3D12Debug> debug_controller{};
        com_ptr<ID3D12InfoQueue1> info_queue{};
        DWORD callback_cookie{};

        if (args.debug) {
            if (SUCCEEDED(D3D12GetDebugInterface(RT_IID_PPV_ARGS(debug_controller)))) {
                debug_controller->EnableDebugLayer();
                dxgi_factory_flags |= DXGI_CREATE_FACTORY_DEBUG;
            }
        }

        com_ptr<IDXGIFactory4> factory{};
        winrt::check_hresult(CreateDXGIFactory2(dxgi_factory_flags, RT_IID_PPV_ARGS(factory)));

        com_ptr<IDXGIAdapter1> adapter{};
        if (!get_adapter(*factory, adapter))
            throw std::exception(
                "Unable to create render context, no graphics device or graphics device does not support");

        com_ptr<ID3D12Device> device{};
        winrt::check_hresult(D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, RT_IID_PPV_ARGS(device)));

        if (args.debug) {
            if (SUCCEEDED(device -> QueryInterface(RT_IID_PPV_ARGS(info_queue)))) {
                if (!SUCCEEDED(
                    info_queue->RegisterMessageCallback(
                        debug_callback, D3D12_MESSAGE_CALLBACK_FLAG_NONE, nullptr, & callback_cookie))) {
                    spdlog::warn("register message callback failed");
                }
            }
        }

        D3D12_COMMAND_QUEUE_DESC queue_desc_direct = {
            .Type = D3D12_COMMAND_LIST_TYPE_DIRECT,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };
        D3D12_COMMAND_QUEUE_DESC queue_desc_compute = {
            .Type = D3D12_COMMAND_LIST_TYPE_COMPUTE,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };
        D3D12_COMMAND_QUEUE_DESC queue_desc_copy = {
            .Type = D3D12_COMMAND_LIST_TYPE_COPY,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };

        com_ptr<ID3D12CommandQueue> command_queue_direct;
        com_ptr<ID3D12CommandQueue> command_queue_compute;
        com_ptr<ID3D12CommandQueue> command_queue_copy;

        winrt::check_hresult(device->CreateCommandQueue(&queue_desc_direct, RT_IID_PPV_ARGS(command_queue_direct)));
        winrt::check_hresult(device->CreateCommandQueue(&queue_desc_compute, RT_IID_PPV_ARGS(command_queue_compute)));
        winrt::check_hresult(device->CreateCommandQueue(&queue_desc_copy, RT_IID_PPV_ARGS(command_queue_copy)));


        auto ctx = std::make_shared<RenderContext>();
        ctx->m_window = window.inner();
        ctx->m_factory = std::move(factory);
        ctx->m_debug_controller = std::move(debug_controller);
        ctx->m_info_queue = std::move(info_queue);
        ctx->m_callback_cookie = callback_cookie;
        ctx->m_adapter = std::move(adapter);
        ctx->m_device = std::move(device);

        ctx->m_surface = std::make_shared<GpuSurface>(ctx->m_factory, ctx->m_device, command_queue_direct, window);

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

        /* 创建队列 */
        {
            com_ptr<ID3D12CommandAllocator> command_allocator_direct;
            com_ptr<ID3D12CommandAllocator> command_allocator_compute;
            com_ptr<ID3D12CommandAllocator> command_allocator_copy;
            com_ptr<ID3D12GraphicsCommandList> command_list_direct;
            com_ptr<ID3D12GraphicsCommandList> command_list_compute;
            com_ptr<ID3D12GraphicsCommandList> command_list_copy;

            winrt::check_hresult(
                ctx->m_device->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE_DIRECT, RT_IID_PPV_ARGS(command_allocator_direct)));

            winrt::check_hresult(
                ctx->m_device->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE_COMPUTE, RT_IID_PPV_ARGS(command_allocator_compute)));

            winrt::check_hresult(
                ctx->m_device->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE_COPY, RT_IID_PPV_ARGS(command_allocator_copy)));

            winrt::check_hresult(
                ctx->m_device->CreateCommandList(
                    0, D3D12_COMMAND_LIST_TYPE_DIRECT,
                    command_allocator_direct.get(), nullptr, RT_IID_PPV_ARGS(command_list_direct)));

            winrt::check_hresult(
                ctx->m_device->CreateCommandList(
                    0, D3D12_COMMAND_LIST_TYPE_COMPUTE,
                    command_allocator_compute.get(), nullptr, RT_IID_PPV_ARGS(command_list_compute)));

            winrt::check_hresult(
                ctx->m_device->CreateCommandList(
                    0, D3D12_COMMAND_LIST_TYPE_COPY,
                    command_allocator_copy.get(), nullptr, RT_IID_PPV_ARGS(command_list_copy)));

            winrt::check_hresult(command_list_direct->Close());
            winrt::check_hresult(command_list_compute->Close());
            winrt::check_hresult(command_list_copy->Close());

            ctx->m_queue_direct = std::make_shared<GpuQueue>(
                ctx->m_device, ctx->m_gpu_allocator, command_allocator_direct,
                command_queue_direct, command_list_direct
            );

            ctx->m_queue_compute = std::make_shared<GpuQueue>(
                ctx->m_device, ctx->m_gpu_allocator, command_allocator_compute,
                command_queue_compute, command_list_compute
            );

            ctx->m_queue_copy = std::make_shared<GpuQueue>(
                ctx->m_device, ctx->m_gpu_allocator, command_allocator_copy,
                command_queue_copy, command_list_copy
            );
        }

        return ctx;
    }

    RenderContext::~RenderContext() {
        m_surface->wait_frame_when_drop(m_queue_direct->m_command_queue);
        if (m_info_queue.get() != nullptr && m_callback_cookie != 0) {
            m_info_queue->UnregisterMessageCallback(m_callback_cookie);
        }
    }

    namespace {
        bool get_adapter(IDXGIFactory1 &factory, com_ptr<IDXGIAdapter1> &adapter) {
            com_ptr<IDXGIFactory6> factory6{};
            if (SUCCEEDED(factory .QueryInterface(RT_IID_PPV_ARGS(factory6)))) {
                for (UINT adapter_index = 0;
                     SUCCEEDED(
                         factory6->EnumAdapterByGpuPreference(
                             adapter_index,
                             DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE,
                             RT_IID_PPV_ARGS(adapter)
                         )
                     );
                     ++adapter_index
                ) {
                    DXGI_ADAPTER_DESC1 desc;
                    adapter->GetDesc1(&desc);

                    if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE) continue;

                    if (SUCCEEDED(
                        D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, _uuidof(ID3D12Device), nullptr)
                    )) {
                        break;
                    }
                }
            }

            if (adapter.get() == nullptr) {
                for (
                    UINT adapter_index = 0;
                    SUCCEEDED(factory.EnumAdapters1(adapter_index, adapter.put()));
                    ++adapter_index
                ) {
                    DXGI_ADAPTER_DESC1 desc;
                    adapter->GetDesc1(&desc);

                    if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE) continue;

                    if (SUCCEEDED(
                        D3D12CreateDevice(adapter.get(), D3D_FEATURE_LEVEL_12_2, _uuidof(ID3D12Device), nullptr)
                    )) {
                        break;
                    }
                }
            }

            return adapter.get() != nullptr;
        }
    }

    void RenderContext::record_frame(const std::function<void(const FrameContext &ctx)> &cb) {
        const auto queue = this->m_queue_direct;
        const auto list = queue->m_command_list;

        // 等待上一帧
        m_surface->wait_frame(queue->m_command_queue);

        // 记录命令
        winrt::check_hresult(queue->m_command_allocator->Reset());
        winrt::check_hresult(list->Reset(queue->m_command_allocator.get(), nullptr));

        GpuCommandList list_box(list);
        cb(FrameContext{*this, *queue, list_box, m_surface});

        if (CD3DX12_RESOURCE_BARRIER barrier; m_surface->require_state(GpuRtState::Present, barrier)) {
            list->ResourceBarrier(1, &barrier);
        }

        winrt::check_hresult(list->Close());

        ID3D12CommandList *lists[] = {list.get()};
        queue->m_command_queue->ExecuteCommandLists(1, lists);

        // 呈现
        m_surface->present();
    }
} // ccc
