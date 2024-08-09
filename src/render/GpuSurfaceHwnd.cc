#include "GpuSurfaceHwnd.h"

#include "Gpu.h"
#include "GpuDevice.h"
#include "GpuQueue.h"
#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuSurfaceHwnd::GpuSurfaceHwnd(
        Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd,
        const int2 size, FError& err
    ) : m_device(std::move(device)), m_queue(std::move(queue)), m_current_size(size)
    {
        if (options.name.ptr != nullptr)
        {
            m_name = std::move(String16::CreateCopy(options.name));
        }

        m_gpu = m_device->m_gpu;

        m_dx_device = m_device->m_device;

        m_v_sync = options.v_sync;

        DXGI_SWAP_CHAIN_DESC1 swap_chain_desc = {};
        swap_chain_desc.BufferCount = FrameCount;
        swap_chain_desc.Width = size.x;
        swap_chain_desc.Height = size.y;
        swap_chain_desc.Format = m_format;
        swap_chain_desc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
        swap_chain_desc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD;
        swap_chain_desc.SampleDesc.Count = 1;

        com_ptr<IDXGISwapChain1> swap_chain;
        winrt::check_hresult(
            m_gpu->m_factory->CreateSwapChainForHwnd(
                m_queue->m_command_queue.get(), // Swap chain needs the queue so that it can force a flush on it.
                hwnd,
                &swap_chain_desc,
                nullptr,
                nullptr,
                swap_chain.put()
            )
        );
        swap_chain.as(m_swap_chain);

        m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

        /* 创建交换链RTV描述符堆 */
        {
            D3D12_DESCRIPTOR_HEAP_DESC rtv_heap_desc = {};
            rtv_heap_desc.NumDescriptors = FrameCount;
            rtv_heap_desc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
            rtv_heap_desc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
            winrt::check_hresult(m_dx_device->CreateDescriptorHeap(&rtv_heap_desc, RT_IID_PPV_ARGS(m_rtv_heap)));
            m_rtv_descriptor_size = m_dx_device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);

            if (options.name.ptr != nullptr)
            {
                winrt::check_hresult(m_rtv_heap->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
            }
        }

        /* 创建帧缓冲区 */
        create_rts();

        /*创建 fence */
        {
            for (int i = 0; i < FrameCount; ++i)
            {
                m_fences[i] = GpuFencePak(m_dx_device, options.name, i);
            }
        }
    }

    void GpuSurfaceHwnd::create_rts()
    {
        CD3DX12_CPU_DESCRIPTOR_HANDLE rtvHandle(m_rtv_heap->GetCPUDescriptorHandleForHeapStart());

        // 为每一帧创建一个 RTV。
        for (UINT n = 0; n < FrameCount; n++)
        {
            winrt::check_hresult(m_swap_chain->GetBuffer(n, RT_IID_PPV_ARGS(m_rts[n])));
            m_dx_device->CreateRenderTargetView(m_rts[n].get(), nullptr, rtvHandle);
            rtvHandle.Offset(1, m_rtv_descriptor_size);

            if (!m_name->is_null())
            {
                winrt::check_hresult(
                    m_rts[n]->SetName(fmt::format(L"{} Frame {}", m_name->c_str(), n).c_str())
                );
            }
        }
    }

    void GpuSurfaceHwnd::wait_all_frame() const
    {
        for (UINT n = 0; n < FrameCount; n++)
        {
            m_fences[n].wait();
        }
    }

    void GpuSurfaceHwnd::move_to_next_frame()
    {
        if (m_resized && (m_current_size.x != m_new_size.x || m_current_size.y != m_new_size.y))
        {
            wait_all_frame();

            m_current_size = m_new_size;

            for (UINT n = 0; n < FrameCount; n++)
            {
                m_rts[n] = nullptr;
            }

            DXGI_SWAP_CHAIN_DESC1 desc = {};
            winrt::check_hresult(m_swap_chain->GetDesc1(&desc));
            winrt::check_hresult(
                m_swap_chain->ResizeBuffers(
                    FrameCount, m_new_size.x, m_new_size.y, desc.Format, desc.Flags
                )
            );

            create_rts();

            m_resized = false;

            m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();
        }
        else
        {
            m_frame_index = m_swap_chain->GetCurrentBackBufferIndex();

            m_fences[m_frame_index].wait();
        }

        m_current_cpu_handle = CD3DX12_CPU_DESCRIPTOR_HANDLE(
            m_rtv_heap->GetCPUDescriptorHandleForHeapStart(), m_frame_index, m_rtv_descriptor_size
        );
    }

    void GpuSurfaceHwnd::present()
    {
        winrt::check_hresult(m_swap_chain->Present(m_v_sync ? 1 : 0, 0));
        m_fences[m_frame_index].signal(m_queue->m_command_queue);
    }

    void GpuSurfaceHwnd::on_resize(const int2 new_size)
    {
        m_resized = true;
        m_new_size = new_size;
    }

    CD3DX12_CPU_DESCRIPTOR_HANDLE GpuSurfaceHwnd::get_dx_cpu_handle() const
    {
        return m_current_cpu_handle;
    }

    Rc<GpuSurfaceHwnd> GpuSurfaceHwnd::Create(
        Rc<GpuDevice> device, const Rc<GpuQueue>& queue, const FGpuSurfaceCreateOptions& options, HWND hwnd, FError& err
    ) noexcept
    {
        int2 size;
        RECT rect;
        if (GetWindowRect(hwnd, &rect))
        {
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            size = {width, height};
        }
        else
        {
            err = make_error(FErrorType::Common, u"Failed to get window size");
            return nullptr;
        }

        try
        {
            Rc r(new GpuSurfaceHwnd(std::move(device), queue, options, hwnd, size, err));
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to create surface!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    GpuSurfaceHwnd::~GpuSurfaceHwnd()
    {
        wait_all_frame();
    }

    FInt2 GpuSurfaceHwnd::get_size() const noexcept
    {
        return {m_current_size.x, m_current_size.y};
    }

    void GpuSurfaceHwnd::ready_frame(FError& err) noexcept
    {
        try
        {
            ready_frame();
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to read frame!");
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
        }
    }

    void GpuSurfaceHwnd::present_frame(FError& err) noexcept
    {
        try
        {
            present();
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to present");
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
        }
    }

    void GpuSurfaceHwnd::ready_frame()
    {
        move_to_next_frame();
    }

    bool GpuSurfaceHwnd::has_rtv() noexcept
    {
        return true;
    }

    bool GpuSurfaceHwnd::has_dsv() noexcept
    {
        return false;
    }

    void GpuSurfaceHwnd::resize(FInt2 new_size) noexcept
    {
        on_resize({new_size.X, new_size.Y});
    }

    bool GpuSurfaceHwnd::get_v_sync() const noexcept
    {
        return m_v_sync;
    }

    void GpuSurfaceHwnd::set_v_sync(const bool v) noexcept
    {
        m_v_sync = v;
    }

    size_t GpuSurfaceHwnd::get_cpu_rtv_handle(FError& err) noexcept
    {
        const D3D12_CPU_DESCRIPTOR_HANDLE h = get_dx_cpu_handle();
        return h.ptr;
    }

    size_t GpuSurfaceHwnd::get_cpu_dsv_handle(FError& err) noexcept
    {
        err = make_error(FErrorType::Gpu, u"Swap chain does not contain depth");
        return 0;
    }

    void* GpuSurfaceHwnd::get_res_raw_ptr() noexcept
    {
        return m_rts[m_frame_index].get();
    }
} // ccc
