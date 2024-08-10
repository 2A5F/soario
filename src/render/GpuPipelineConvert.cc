#include "GpuPipelineConvert.h"

namespace ccc
{
    D3D12_BLEND to_dx(const FGpuPipelineBlendType type)
    {
        switch (type)
        {
        case FGpuPipelineBlendType::Zero:
            return D3D12_BLEND_ZERO;
        case FGpuPipelineBlendType::One:
            return D3D12_BLEND_ONE;
        case FGpuPipelineBlendType::SrcColor:
            return D3D12_BLEND_SRC_COLOR;
        case FGpuPipelineBlendType::InvSrcColor:
            return D3D12_BLEND_INV_SRC_COLOR;
        case FGpuPipelineBlendType::SrcAlpha:
            return D3D12_BLEND_SRC_ALPHA;
        case FGpuPipelineBlendType::InvSrcAlpha:
            return D3D12_BLEND_INV_SRC_ALPHA;
        case FGpuPipelineBlendType::DstAlpha:
            return D3D12_BLEND_DEST_ALPHA;
        case FGpuPipelineBlendType::InvDstAlpha:
            return D3D12_BLEND_INV_DEST_ALPHA;
        case FGpuPipelineBlendType::DstColor:
            return D3D12_BLEND_DEST_COLOR;
        case FGpuPipelineBlendType::InvDstColor:
            return D3D12_BLEND_INV_DEST_COLOR;
        case FGpuPipelineBlendType::SrcAlphaSat:
            return D3D12_BLEND_SRC_ALPHA_SAT;
        case FGpuPipelineBlendType::BlendFactor:
            return D3D12_BLEND_BLEND_FACTOR;
        case FGpuPipelineBlendType::BlendInvBlendFactor:
            return D3D12_BLEND_INV_BLEND_FACTOR;
        case FGpuPipelineBlendType::Src1Color:
            return D3D12_BLEND_SRC1_ALPHA;
        case FGpuPipelineBlendType::InvSrc1Color:
            return D3D12_BLEND_INV_SRC1_ALPHA;
        case FGpuPipelineBlendType::Src1Alpha:
            return D3D12_BLEND_SRC1_ALPHA;
        case FGpuPipelineBlendType::InvSrc1Alpha:
            return D3D12_BLEND_INV_SRC1_ALPHA;
        case FGpuPipelineBlendType::AlphaFactor:
            return D3D12_BLEND_ALPHA_FACTOR;
        case FGpuPipelineBlendType::InvAlphaFactor:
            return D3D12_BLEND_INV_ALPHA_FACTOR;
        default:
            return D3D12_BLEND_ZERO;
        }
    }

    D3D12_BLEND_OP to_dx(const FGpuPipelineBlendOp op)
    {
        switch (op)
        {
        case FGpuPipelineBlendOp::Add:
            return D3D12_BLEND_OP_ADD;
        case FGpuPipelineBlendOp::Sub:
            return D3D12_BLEND_OP_SUBTRACT;
        case FGpuPipelineBlendOp::RevSub:
            return D3D12_BLEND_OP_REV_SUBTRACT;
        case FGpuPipelineBlendOp::Min:
            return D3D12_BLEND_OP_MIN;
        case FGpuPipelineBlendOp::Max:
            return D3D12_BLEND_OP_MAX;
        default:
            return D3D12_BLEND_OP_ADD;
        }
    }

    D3D12_LOGIC_OP to_dx(const FGpuPipelineLogicOp op)
    {
        switch (op)
        {
        case FGpuPipelineLogicOp::Clear:
            return D3D12_LOGIC_OP_CLEAR;
        case FGpuPipelineLogicOp::One:
            return D3D12_LOGIC_OP_SET;
        case FGpuPipelineLogicOp::Copy:
            return D3D12_LOGIC_OP_COPY;
        case FGpuPipelineLogicOp::CopyInv:
            return D3D12_LOGIC_OP_COPY_INVERTED;
        case FGpuPipelineLogicOp::Noop:
            return D3D12_LOGIC_OP_NOOP;
        case FGpuPipelineLogicOp::Inv:
            return D3D12_LOGIC_OP_INVERT;
        case FGpuPipelineLogicOp::And:
            return D3D12_LOGIC_OP_AND;
        case FGpuPipelineLogicOp::NAnd:
            return D3D12_LOGIC_OP_NAND;
        case FGpuPipelineLogicOp::Or:
            return D3D12_LOGIC_OP_OR;
        case FGpuPipelineLogicOp::Nor:
            return D3D12_LOGIC_OP_NOR;
        case FGpuPipelineLogicOp::Xor:
            return D3D12_LOGIC_OP_XOR;
        case FGpuPipelineLogicOp::Equiv:
            return D3D12_LOGIC_OP_EQUIV;
        case FGpuPipelineLogicOp::AndRev:
            return D3D12_LOGIC_OP_AND_REVERSE;
        case FGpuPipelineLogicOp::AndInv:
            return D3D12_LOGIC_OP_AND_INVERTED;
        case FGpuPipelineLogicOp::OrRev:
            return D3D12_LOGIC_OP_OR_REVERSE;
        case FGpuPipelineLogicOp::OrInv:
            return D3D12_LOGIC_OP_INVERT;
        default:
            return D3D12_LOGIC_OP_CLEAR;
        }
    }

    UINT8 to_dx(const FGpuPipelineColorMask mask)
    {
        return *reinterpret_cast<const UINT8*>(&mask);
    }

    void to_dx(
        D3D12_BLEND_DESC& desc, const FGpuPipelineBlendState& state, const int32_t rt_count
    )
    {
        desc.AlphaToCoverageEnable = state.alpha_to_coverage == FGpuPipelineSwitch::On;
        desc.IndependentBlendEnable = state.independent_blend == FGpuPipelineSwitch::On;
        for (int i = 0; i < rt_count; ++i)
        {
            auto& dst = desc.RenderTarget[i];
            const auto& src = state.rts[i];
            if (src.blend != FGpuPipelineSwitch::On) continue;
            dst.BlendEnable = true;
            dst.SrcBlend = to_dx(src.src_blend);
            dst.DestBlend = to_dx(src.dst_blend);
            dst.BlendOp = to_dx(src.blend_op);
            dst.SrcBlendAlpha = to_dx(src.src_alpha_blend);
            dst.DestBlendAlpha = to_dx(src.dst_alpha_blend);
            dst.BlendOpAlpha = to_dx(src.alpha_blend_op);
            dst.LogicOpEnable = src.logic_op != FGpuPipelineLogicOp::None;
            if (dst.LogicOpEnable) dst.LogicOp = to_dx(src.logic_op);
            dst.RenderTargetWriteMask = to_dx(src.write_mask);
        }
    }

    D3D12_FILL_MODE to_dx(const FGpuPipelineFillMode mode)
    {
        switch (mode)
        {
        case FGpuPipelineFillMode::WireFrame:
            return D3D12_FILL_MODE_WIREFRAME;
        case FGpuPipelineFillMode::Solid:
        default:
            return D3D12_FILL_MODE_SOLID;
        }
    }

    D3D12_CULL_MODE to_dx(const FGpuPipelineCullMode mode)
    {
        switch (mode)
        {
        case FGpuPipelineCullMode::Off:
            return D3D12_CULL_MODE_NONE;
        case FGpuPipelineCullMode::Front:
            return D3D12_CULL_MODE_FRONT;
        case FGpuPipelineCullMode::Back:
        default:
            return D3D12_CULL_MODE_BACK;
        }
    }

    void to_dx(D3D12_RASTERIZER_DESC& desc, const FGpuPipelineRasterizerState& state)
    {
        desc.FillMode = to_dx(state.fill_mode);
        desc.CullMode = to_dx(state.cull_mode);
        desc.DepthClipEnable = state.depth_clip == FGpuPipelineSwitch::On;
        desc.MultisampleEnable = state.multisample == FGpuPipelineSwitch::On;
        desc.ForcedSampleCount = state.forced_sample_count;
        desc.DepthBias = state.depth_bias;
        desc.DepthBiasClamp = state.depth_bias_clamp;
        desc.SlopeScaledDepthBias = state.slope_scaled_depth_bias;
        desc.AntialiasedLineEnable = state.aa_line == FGpuPipelineSwitch::On;
        desc.ConservativeRaster = state.conservative == FGpuPipelineSwitch::On
            ? D3D12_CONSERVATIVE_RASTERIZATION_MODE_ON
            : D3D12_CONSERVATIVE_RASTERIZATION_MODE_OFF;
    }

    D3D12_COMPARISON_FUNC to_dx(const FGpuPipelineCmpFunc func)
    {
        switch (func)
        {
        case FGpuPipelineCmpFunc::Less:
            return D3D12_COMPARISON_FUNC_LESS;
        case FGpuPipelineCmpFunc::Equal:
            return D3D12_COMPARISON_FUNC_EQUAL;
        case FGpuPipelineCmpFunc::LessEqual:
            return D3D12_COMPARISON_FUNC_LESS_EQUAL;
        case FGpuPipelineCmpFunc::Greater:
            return D3D12_COMPARISON_FUNC_GREATER;
        case FGpuPipelineCmpFunc::NotEqual:
            return D3D12_COMPARISON_FUNC_NOT_EQUAL;
        case FGpuPipelineCmpFunc::GreaterEqual:
            return D3D12_COMPARISON_FUNC_GREATER_EQUAL;
        case FGpuPipelineCmpFunc::Always:
            return D3D12_COMPARISON_FUNC_ALWAYS;
        default:
        case FGpuPipelineCmpFunc::Never:
            return D3D12_COMPARISON_FUNC_NEVER;
        }
    }

    D3D12_STENCIL_OP to_dx(const FGpuPipelineStencilFailOp op)
    {
        switch (op)
        {
        case FGpuPipelineStencilFailOp::Keep:
            return D3D12_STENCIL_OP_KEEP;
        case FGpuPipelineStencilFailOp::Zero:
            return D3D12_STENCIL_OP_ZERO;
        case FGpuPipelineStencilFailOp::Replace:
            return D3D12_STENCIL_OP_REPLACE;
        case FGpuPipelineStencilFailOp::IncSat:
            return D3D12_STENCIL_OP_INCR_SAT;
        case FGpuPipelineStencilFailOp::DecSat:
            return D3D12_STENCIL_OP_DECR_SAT;
        case FGpuPipelineStencilFailOp::Invert:
            return D3D12_STENCIL_OP_INVERT;
        case FGpuPipelineStencilFailOp::Inc:
            return D3D12_STENCIL_OP_INCR;
        case FGpuPipelineStencilFailOp::Dec:
            return D3D12_STENCIL_OP_DECR;
        default:
            return D3D12_STENCIL_OP_KEEP;
        }
    }

    void to_dx(D3D12_DEPTH_STENCILOP_DESC& desc, const FGpuPipelineStencilState& state)
    {
        desc.StencilPassOp = to_dx(state.pass_op);
        desc.StencilFailOp = to_dx(state.fail_op);
        desc.StencilDepthFailOp = to_dx(state.depth_fail_op);
        desc.StencilFunc = to_dx(state.func);
    }

    void to_dx(D3D12_DEPTH_STENCIL_DESC& desc, const FGpuPipelineDepthStencilState& state)
    {
        desc.DepthEnable = state.depth_func != FGpuPipelineCmpFunc::Off;
        desc.DepthWriteMask = state.depth_write_mask == FGpuPipelineDepthWriteMask::All
            ? D3D12_DEPTH_WRITE_MASK_ALL
            : D3D12_DEPTH_WRITE_MASK_ZERO;
        desc.DepthFunc = to_dx(state.depth_func);
        desc.StencilEnable = state.stencil_enable == FGpuPipelineSwitch::On;
        desc.StencilReadMask = state.stencil_read_mask;
        desc.StencilWriteMask = state.stencil_write_mask;
        to_dx(desc.FrontFace, state.front_face);
        to_dx(desc.BackFace, state.back_face);
    }

    D3D12_PRIMITIVE_TOPOLOGY_TYPE to_dx(const FGpuPipelinePrimitiveTopologyType topology)
    {
        switch (topology)
        {
        case FGpuPipelinePrimitiveTopologyType::Point:
            return D3D12_PRIMITIVE_TOPOLOGY_TYPE_POINT;
        case FGpuPipelinePrimitiveTopologyType::Line:
            return D3D12_PRIMITIVE_TOPOLOGY_TYPE_LINE;
        case FGpuPipelinePrimitiveTopologyType::Triangle:
            return D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE;
        case FGpuPipelinePrimitiveTopologyType::Patch:
            return D3D12_PRIMITIVE_TOPOLOGY_TYPE_PATCH;
        case FGpuPipelinePrimitiveTopologyType::Undefined:
        default:
            return D3D12_PRIMITIVE_TOPOLOGY_TYPE_UNDEFINED;
        }
    }

    DXGI_FORMAT to_dx(const FGpuTextureFormat format)
    {
        switch (format)
        {
        case FGpuTextureFormat::R32G32B32A32_TypeLess:
            return DXGI_FORMAT_R32G32B32A32_TYPELESS;
        case FGpuTextureFormat::R32G32B32A32_Float:
            return DXGI_FORMAT_R32G32B32A32_FLOAT;
        case FGpuTextureFormat::R32G32B32A32_UInt:
            return DXGI_FORMAT_R32G32B32A32_UINT;
        case FGpuTextureFormat::R32G32B32A32_SInt:
            return DXGI_FORMAT_R32G32B32A32_SINT;
        case FGpuTextureFormat::R32G32B32_TypeLess:
            return DXGI_FORMAT_R32G32B32_TYPELESS;
        case FGpuTextureFormat::R32G32B32_Float:
            return DXGI_FORMAT_R32G32B32_FLOAT;
        case FGpuTextureFormat::R32G32B32_UInt:
            return DXGI_FORMAT_R32G32B32_UINT;
        case FGpuTextureFormat::R32G32B32_SInt:
            return DXGI_FORMAT_R32G32B32_SINT;
        case FGpuTextureFormat::R16G16B16A16_TypeLess:
            return DXGI_FORMAT_R16G16B16A16_TYPELESS;
        case FGpuTextureFormat::R16G16B16A16_Float:
            return DXGI_FORMAT_R16G16B16A16_FLOAT;
        case FGpuTextureFormat::R16G16B16A16_UNorm:
            return DXGI_FORMAT_R16G16B16A16_UNORM;
        case FGpuTextureFormat::R16G16B16A16_UInt:
            return DXGI_FORMAT_R16G16B16A16_UINT;
        case FGpuTextureFormat::R16G16B16A16_SNorm:
            return DXGI_FORMAT_R16G16B16A16_SNORM;
        case FGpuTextureFormat::R16G16B16A16_SInt:
            return DXGI_FORMAT_R16G16B16A16_SINT;
        case FGpuTextureFormat::R32G32_TypeLess:
            return DXGI_FORMAT_R32G32_TYPELESS;
        case FGpuTextureFormat::R32G32_Float:
            return DXGI_FORMAT_R32G32_FLOAT;
        case FGpuTextureFormat::R32G32_UInt:
            return DXGI_FORMAT_R32G32_UINT;
        case FGpuTextureFormat::R32G32_SInt:
            return DXGI_FORMAT_R32G32_SINT;
        case FGpuTextureFormat::R32G8X24_TypeLess:
            return DXGI_FORMAT_R32G8X24_TYPELESS;
        case FGpuTextureFormat::D32_Float_S8X24_UInt:
            return DXGI_FORMAT_D32_FLOAT_S8X24_UINT;
        case FGpuTextureFormat::R32_Float_X8X24_TypeLess:
            return DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS;
        case FGpuTextureFormat::X32_TypeLess_G8X24_Float:
            return DXGI_FORMAT_X32_TYPELESS_G8X24_UINT;
        case FGpuTextureFormat::R10G10B10A2_TypeLess:
            return DXGI_FORMAT_R10G10B10A2_TYPELESS;
        case FGpuTextureFormat::R10G10B10A2_UNorm:
            return DXGI_FORMAT_R10G10B10A2_UNORM;
        case FGpuTextureFormat::R10G10B10A2_UInt:
            return DXGI_FORMAT_R10G10B10A2_UINT;
        case FGpuTextureFormat::R11G11B10_Float:
            return DXGI_FORMAT_R11G11B10_FLOAT;
        case FGpuTextureFormat::R8G8B8A8_TypeLess:
            return DXGI_FORMAT_R8G8B8A8_TYPELESS;
        case FGpuTextureFormat::R8G8B8A8_UNorm:
            return DXGI_FORMAT_R8G8B8A8_UNORM;
        case FGpuTextureFormat::R8G8B8A8_UNorm_sRGB:
            return DXGI_FORMAT_R8G8B8A8_UNORM_SRGB;
        case FGpuTextureFormat::R8G8B8A8_UInt:
            return DXGI_FORMAT_R8G8B8A8_UINT;
        case FGpuTextureFormat::R8G8B8A8_SNorm:
            return DXGI_FORMAT_R8G8B8A8_SNORM;
        case FGpuTextureFormat::R8G8B8A8_SInt:
            return DXGI_FORMAT_R8G8B8A8_SINT;
        case FGpuTextureFormat::R16G16_TypeLess:
            return DXGI_FORMAT_R16G16_TYPELESS;
        case FGpuTextureFormat::R16G16_Float:
            return DXGI_FORMAT_R16G16_FLOAT;
        case FGpuTextureFormat::R16G16_UNorm:
            return DXGI_FORMAT_R16G16_UNORM;
        case FGpuTextureFormat::R16G16_UInt:
            return DXGI_FORMAT_R16G16_UINT;
        case FGpuTextureFormat::R16G16_SNorm:
            return DXGI_FORMAT_R16G16_SNORM;
        case FGpuTextureFormat::R16G16_SInt:
            return DXGI_FORMAT_R16G16_SINT;
        case FGpuTextureFormat::R32_TypeLess:
            return DXGI_FORMAT_R32_TYPELESS;
        case FGpuTextureFormat::D32_Float:
            return DXGI_FORMAT_D32_FLOAT;
        case FGpuTextureFormat::R32_Float:
            return DXGI_FORMAT_R32_FLOAT;
        case FGpuTextureFormat::R32_UInt:
            return DXGI_FORMAT_R32_UINT;
        case FGpuTextureFormat::R32_SInt:
            return DXGI_FORMAT_R32_SINT;
        case FGpuTextureFormat::R24G8_TypeLess:
            return DXGI_FORMAT_R24G8_TYPELESS;
        case FGpuTextureFormat::D24_UNorm_S8_UInt:
            return DXGI_FORMAT_D24_UNORM_S8_UINT;
        case FGpuTextureFormat::R24_UNorm_X8_TypeLess:
            return DXGI_FORMAT_R24_UNORM_X8_TYPELESS;
        case FGpuTextureFormat::X24_TypeLess_G8_UInt:
            return DXGI_FORMAT_X24_TYPELESS_G8_UINT;
        case FGpuTextureFormat::R8G8_TypeLess:
            return DXGI_FORMAT_R8G8_TYPELESS;
        case FGpuTextureFormat::R8G8_UNorm:
            return DXGI_FORMAT_R8G8_UNORM;
        case FGpuTextureFormat::R8G8_UInt:
            return DXGI_FORMAT_R8G8_UINT;
        case FGpuTextureFormat::R8G8_SNorm:
            return DXGI_FORMAT_R8G8_SNORM;
        case FGpuTextureFormat::R8G8_SInt:
            return DXGI_FORMAT_R8G8_SINT;
        case FGpuTextureFormat::R16_TypeLess:
            return DXGI_FORMAT_R16_TYPELESS;
        case FGpuTextureFormat::R16_Float:
            return DXGI_FORMAT_R16_FLOAT;
        case FGpuTextureFormat::D16_UNorm:
            return DXGI_FORMAT_D16_UNORM;
        case FGpuTextureFormat::R16_UNorm:
            return DXGI_FORMAT_R16_UNORM;
        case FGpuTextureFormat::R16_UInt:
            return DXGI_FORMAT_R16_UINT;
        case FGpuTextureFormat::R16_SNorm:
            return DXGI_FORMAT_R16_SNORM;
        case FGpuTextureFormat::R16_SInt:
            return DXGI_FORMAT_R16_SINT;
        case FGpuTextureFormat::R8_TypeLess:
            return DXGI_FORMAT_R8_TYPELESS;
        case FGpuTextureFormat::R8_UNorm:
            return DXGI_FORMAT_R8_UNORM;
        case FGpuTextureFormat::R8_UInt:
            return DXGI_FORMAT_R8_UINT;
        case FGpuTextureFormat::R8_SNorm:
            return DXGI_FORMAT_R8_SNORM;
        case FGpuTextureFormat::R8_SInt:
            return DXGI_FORMAT_R8_SINT;
        case FGpuTextureFormat::A8_UNorm:
            return DXGI_FORMAT_A8_UNORM;
        case FGpuTextureFormat::R1_UNorm:
            return DXGI_FORMAT_R1_UNORM;
        case FGpuTextureFormat::R9G9B9E5_SharedExp:
            return DXGI_FORMAT_R9G9B9E5_SHAREDEXP;
        case FGpuTextureFormat::R8G8_B8G8_UNorm:
            return DXGI_FORMAT_R8G8_B8G8_UNORM;
        case FGpuTextureFormat::G8R8_G8B8_UNorm:
            return DXGI_FORMAT_G8R8_G8B8_UNORM;
        case FGpuTextureFormat::BC1_TypeLess:
            return DXGI_FORMAT_BC1_TYPELESS;
        case FGpuTextureFormat::BC1_UNorm:
            return DXGI_FORMAT_BC1_UNORM;
        case FGpuTextureFormat::BC1_UNorm_sRGB:
            return DXGI_FORMAT_BC1_UNORM_SRGB;
        case FGpuTextureFormat::BC2_TypeLess:
            return DXGI_FORMAT_BC2_TYPELESS;
        case FGpuTextureFormat::BC2_UNorm:
            return DXGI_FORMAT_BC2_UNORM;
        case FGpuTextureFormat::BC2_UNorm_sRGB:
            return DXGI_FORMAT_BC2_UNORM_SRGB;
        case FGpuTextureFormat::BC3_TypeLess:
            return DXGI_FORMAT_BC3_TYPELESS;
        case FGpuTextureFormat::BC3_UNorm:
            return DXGI_FORMAT_BC3_UNORM;
        case FGpuTextureFormat::BC3_UNorm_sRGB:
            return DXGI_FORMAT_BC3_UNORM_SRGB;
        case FGpuTextureFormat::BC4_TypeLess:
            return DXGI_FORMAT_BC4_TYPELESS;
        case FGpuTextureFormat::BC4_UNorm:
            return DXGI_FORMAT_BC4_UNORM;
        case FGpuTextureFormat::BC4_SNorm:
            return DXGI_FORMAT_BC4_SNORM;
        case FGpuTextureFormat::BC5_TypeLess:
            return DXGI_FORMAT_BC5_TYPELESS;
        case FGpuTextureFormat::BC5_UNorm:
            return DXGI_FORMAT_BC5_UNORM;
        case FGpuTextureFormat::BC5_SNorm:
            return DXGI_FORMAT_BC5_SNORM;
        case FGpuTextureFormat::B5G6R5_UNorm:
            return DXGI_FORMAT_B5G6R5_UNORM;
        case FGpuTextureFormat::B5G5R5A1_UNorm:
            return DXGI_FORMAT_B5G5R5A1_UNORM;
        case FGpuTextureFormat::B8G8R8A8_UNorm:
            return DXGI_FORMAT_B8G8R8A8_UNORM;
        case FGpuTextureFormat::B8G8R8X8_UNorm:
            return DXGI_FORMAT_B8G8R8X8_UNORM;
        case FGpuTextureFormat::R10G10B10_XR_Bias_A2_UNorm:
            return DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM;
        case FGpuTextureFormat::B8G8R8A8_TypeLess:
            return DXGI_FORMAT_B8G8R8A8_TYPELESS;
        case FGpuTextureFormat::B8G8R8A8_UNorm_sRGB:
            return DXGI_FORMAT_B8G8R8A8_UNORM_SRGB;
        case FGpuTextureFormat::B8G8R8X8_TypeLess:
            return DXGI_FORMAT_B8G8R8X8_TYPELESS;
        case FGpuTextureFormat::B8G8R8X8_UNorm_sRGB:
            return DXGI_FORMAT_B8G8R8X8_UNORM_SRGB;
        case FGpuTextureFormat::BC6H_TypeLess:
            return DXGI_FORMAT_BC6H_TYPELESS;
        case FGpuTextureFormat::BC6H_UF16:
            return DXGI_FORMAT_BC6H_UF16;
        case FGpuTextureFormat::BC6H_SF16:
            return DXGI_FORMAT_BC6H_SF16;
        case FGpuTextureFormat::BC7_TypeLess:
            return DXGI_FORMAT_BC7_TYPELESS;
        case FGpuTextureFormat::BC7_UNorm:
            return DXGI_FORMAT_BC7_UNORM;
        case FGpuTextureFormat::BC7_UNorm_sRGB:
            return DXGI_FORMAT_BC7_UNORM_SRGB;
        case FGpuTextureFormat::Unknown:
        default:
            return DXGI_FORMAT_UNKNOWN;
        }
    }
} // ccc
