#include "./GpuDevice.h"
#include "./Gpu.h"
#include "../Args.h"
#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    void GpuDevice::debug_callback(
        D3D12_MESSAGE_CATEGORY Category, D3D12_MESSAGE_SEVERITY Severity, D3D12_MESSAGE_ID ID, LPCSTR pDescription,
        void* pContext
    )
    {
        const auto self = static_cast<GpuDevice*>(pContext);
        if (self->m_logger != nullptr)
        {
            FLogLevel level;
            if (Severity <= D3D12_MESSAGE_SEVERITY_ERROR)
            {
                level = FLogLevel::Error;
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_WARNING)
            {
                level = FLogLevel::Warn;
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_INFO)
            {
                level = FLogLevel::Info;
            }
            else
            {
                level = FLogLevel::Debug;
            }

            self->m_logger(self->m_logger_object, level, pDescription);
        }
        else
        {
            if (Severity <= D3D12_MESSAGE_SEVERITY_ERROR)
            {
                logger::error(pDescription);
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_WARNING)
            {
                logger::warn(pDescription);
            }
            else if (Severity == D3D12_MESSAGE_SEVERITY_INFO)
            {
                logger::info(pDescription);
            }
            else
            {
                logger::debug(pDescription);
            }
        }
    }

    GpuDevice::GpuDevice(Rc<Gpu> gpu, com_ptr<ID3D12Device> device, const FGpuDeviceCreateOptions& options) : m_gpu(
            std::move(gpu)
        ), m_device(std::move(device)),
        m_logger(options.logger),
        m_logger_object(options.logger_object),
        m_logger_drop_object(options.logger_drop_object)
    {
        /* 创建分配器 */
        {
            D3D12MA::ALLOCATOR_DESC allocator_desc = {};
            allocator_desc.pDevice = m_device.get();
            allocator_desc.pAdapter = m_gpu->m_adapter.get();
            allocator_desc.Flags =
                D3D12MA::ALLOCATOR_FLAG_MSAA_TEXTURES_ALWAYS_COMMITTED |
                D3D12MA::ALLOCATOR_FLAG_DEFAULT_POOLS_NOT_ZEROED;
            winrt::check_hresult(CreateAllocator(&allocator_desc, m_gpu_allocator.put()));
        }

        const auto& args = Args::get();

        if (args.debug)
        {
            if (SUCCEEDED(m_device -> QueryInterface(RT_IID_PPV_ARGS(m_info_queue))))
            {
                if (!SUCCEEDED(
                    m_info_queue->RegisterMessageCallback(
                        debug_callback, D3D12_MESSAGE_CALLBACK_FLAG_NONE, this, & m_callback_cookie)
                ))
                {
                    logger::warn("register message callback failed");
                }
            }
        }
    }

    GpuDevice::~GpuDevice()
    {
        if (m_info_queue.get() != nullptr && m_callback_cookie != 0)
        {
            m_info_queue->UnregisterMessageCallback(m_callback_cookie);
        }
        if (m_logger_object != nullptr && m_logger_drop_object != nullptr)
        {
            m_logger_drop_object(m_logger_object);
        }
    }

    Rc<GpuDevice> GpuDevice::Create(Rc<Gpu> gpu, const FGpuDeviceCreateOptions& options, FError& err) noexcept
    {
        try
        {
            com_ptr<ID3D12Device> device{};
            winrt::check_hresult(
                D3D12CreateDevice(gpu->m_adapter.get(), D3D_FEATURE_LEVEL_12_2, RT_IID_PPV_ARGS(device))
            );

            if (options.name.ptr != nullptr)
            {
                winrt::check_hresult(device->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
            }

            /* 检查设备支持 */
            {
                D3D12_FEATURE_DATA_SHADER_MODEL shader_model = {D3D_SHADER_MODEL_6_6};
                if (FAILED(device->CheckFeatureSupport(D3D12_FEATURE_SHADER_MODEL, &shader_model, sizeof(shader_model)))
                    || (shader_model.HighestShaderModel < D3D_SHADER_MODEL_6_6))
                {
                    err = make_error(FErrorType::Gpu, "Shader Model 6.6 is not supported");
                    return nullptr;
                }

                D3D12_FEATURE_DATA_D3D12_OPTIONS7 features = {};
                if (FAILED(device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS7, &features, sizeof(features)))
                    || (features.MeshShaderTier == D3D12_MESH_SHADER_TIER_NOT_SUPPORTED))
                {
                    err = make_error(FErrorType::Gpu, "Mesh Shaders aren't supported!");
                    return nullptr;
                }
            }

            Rc r(new GpuDevice(std::move(gpu), std::move(device), options));
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, "Failed to create device!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    FGpuQueue* GpuDevice::CreateQueue(const FGpuQueueCreateOptions& options, FError& err) noexcept
    {
        Rc r = GpuQueue::Create(Rc<GpuDevice>::UnsafeClone(this), options, err);
        return r.leak();
    }
} // ccc
