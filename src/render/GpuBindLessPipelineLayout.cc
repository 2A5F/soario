#include "GpuBindLessPipelineLayout.h"

#include "directxtk12/DirectXHelpers.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    namespace static_samplers
    {
        constexpr D3D12_STATIC_SAMPLER_DESC s_static_samplers[] = {
            /* point clamp */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_POINT,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 0,
            },
            /* point wrap */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_POINT,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 1,
            },
            /* point mirror */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_POINT,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 2,
            },
            /* liner clamp */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_LINEAR,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_CLAMP,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 3,
            },
            /* liner wrap */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_LINEAR,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_WRAP,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 4,
            },
            /* liner mirror */ {
                .Filter = D3D12_FILTER_MIN_MAG_MIP_LINEAR,
                .AddressU = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .AddressV = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .AddressW = D3D12_TEXTURE_ADDRESS_MODE_MIRROR,
                .MaxAnisotropy = 1,
                .MinLOD = -FLT_MAX,
                .MaxLOD = FLT_MAX,
                .ShaderRegister = 5,
            },
        };
    }

    GpuBindLessPipelineLayout::GpuBindLessPipelineLayout(
        Rc<GpuDevice> device, const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
    ) : m_device(std::move(device))
    {
        D3D12_ROOT_SIGNATURE_DESC desc = {};
        desc.Flags = D3D12_ROOT_SIGNATURE_FLAG_CBV_SRV_UAV_HEAP_DIRECTLY_INDEXED
            | D3D12_ROOT_SIGNATURE_FLAG_SAMPLER_HEAP_DIRECTLY_INDEXED;
        desc.pStaticSamplers = static_samplers::s_static_samplers;
        desc.NumStaticSamplers = std::size(static_samplers::s_static_samplers);
        D3D12_ROOT_PARAMETER root_parameter[1] = {};
        root_parameter[0].ParameterType = D3D12_ROOT_PARAMETER_TYPE_32BIT_CONSTANTS;
        desc.pParameters = root_parameter;
        desc.NumParameters = std::size(root_parameter);
        winrt::check_hresult(DirectX::CreateRootSignature(m_device->m_device.get(), &desc, m_root_signature.put()));
        if (options.name.ptr != nullptr)
        {
            winrt::check_hresult(m_root_signature->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
        }
    }

    Rc<GpuBindLessPipelineLayout> GpuBindLessPipelineLayout::Create(
        Rc<GpuDevice> device, const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
    ) noexcept
    {
        try
        {
            Rc r(new GpuBindLessPipelineLayout(std::move(device), options, err));
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, "Failed to create bindLess pipeline layout");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    void* GpuBindLessPipelineLayout::get_raw_ptr() const noexcept
    {
        return m_root_signature.get();
    }
} // ccc
