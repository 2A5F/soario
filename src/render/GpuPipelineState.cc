#include "GpuPipelineState.h"

#include <directx/d3dx12_pipeline_state_stream.h>

#include "GpuDevice.h"
#include "GpuPipelineLayout.h"

#include "GpuPipelineConvert.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuPipelineState::GpuPipelineState(
        Rc<GpuDevice> device, Rc<GpuPipelineLayout> layout, com_ptr<ID3D12PipelineState> pipeline_state
    ) : m_device(std::move(device)), m_layout(std::move(layout)), m_pipeline_state(std::move(pipeline_state))
    {
    }

    Rc<GpuPipelineState> GpuPipelineState::Create(
        Rc<GpuDevice> device, Rc<GpuPipelineLayout> layout, const FGpuPipelineStateCreateOptions& options, FError& err
    ) noexcept
    {
        try
        {
            const auto flag = options.flag;
            if (!flag.bind_less)
            {
                err = make_error(FErrorType::Gpu, u"Currently only bind less is supported");
                return nullptr;
            }

            com_ptr<ID3D12PipelineState> pipeline_state;

            if (flag.ps)
            {
                if (flag.ms)
                {
                    D3DX12_MESH_SHADER_PIPELINE_STATE_DESC desc = {};
                    desc.pRootSignature = layout->get_root_signature().get();
                    desc.PS = {options.blob[0].ptr, options.blob[0].len};
                    desc.MS = {options.blob[1].ptr, options.blob[1].len};
                    if (flag.ts)
                    {
                        desc.AS = {options.blob[2].ptr, options.blob[2].len};
                    }
                    to_dx(desc.BlendState, options.blend_state, options.rt_count);
                    desc.SampleMask = options.sample_mask;
                    to_dx(desc.RasterizerState, options.rasterizer_state);
                    to_dx(desc.DepthStencilState, options.depth_stencil_state);
                    desc.PrimitiveTopologyType = to_dx(options.primitive_topology_type);
                    desc.NumRenderTargets = options.rt_count;
                    desc.SampleDesc.Count = options.sample_state.count;
                    desc.SampleDesc.Quality = options.sample_state.quality;
                    for (int i = 0; i < options.rt_count; ++i)
                    {
                        desc.RTVFormats[i] = to_dx(options.rtv_formats[i]);
                    }
                    desc.DSVFormat = to_dx(options.dsv_format);

                    auto pso_stream = CD3DX12_PIPELINE_MESH_STATE_STREAM(desc);

                    D3D12_PIPELINE_STATE_STREAM_DESC stream_desc;
                    stream_desc.pPipelineStateSubobjectStream = &pso_stream;
                    stream_desc.SizeInBytes = sizeof(pso_stream);

                    winrt::check_hresult(
                        device->m_device->CreatePipelineState(&stream_desc, RT_IID_PPV_ARGS(pipeline_state))
                    );
                    goto ok;
                }
            }

            err = make_error(FErrorType::Gpu, u"This shader combination is currently not supported");
            return nullptr;

        ok:
            if (options.name.ptr != nullptr)
            {
                winrt::check_hresult(pipeline_state->SetName(reinterpret_cast<const wchar_t*>(options.name.ptr)));
            }
            Rc r(new GpuPipelineState(std::move(device), std::move(layout), std::move(pipeline_state)));
            if (err.type != FErrorType::None) return nullptr;
            return r;
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to create pipeline state");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    FGpuPipelineLayout* GpuPipelineState::get_layout_ref() const noexcept
    {
        return m_layout.get();
    }

    void* GpuPipelineState::get_raw_ptr() const noexcept
    {
        return m_pipeline_state.get();
    }
} // ccc
