#pragma once
#include "FGpuFormat.h"
#include "FGpuPipelineLayout.h"
#include "../FFI.h"

namespace ccc
{
    struct FGpuPipelineCreateFlag
    {
        uint16_t bind_less : 1;
        uint16_t cs        : 1;
        uint16_t ps        : 1;
        uint16_t vs        : 1;
        uint16_t ms        : 1;
        uint16_t ts        : 1;
    };

    enum class FGpuPipelinePrimitiveTopologyType : uint8_t
    {
        Undefined,
        Point,
        Line,
        Triangle,
        Patch,
    };

    struct FGpuPipelineColorMask
    {
        uint8_t r : 1;
        uint8_t g : 1;
        uint8_t b : 1;
        uint8_t a : 1;

        bool all() const { return r && g && b && a; }
    };

    /* 语义化的 bool 值 */
    enum class FGpuPipelineSwitch : uint8_t
    {
        /* 关闭 */
        Off = 0,
        /* 启用 */
        On = 1,
    };

    /* 填充模式 */
    enum class FGpuPipelineFillMode : uint8_t
    {
        /* 绘制连接顶点的线条， 不绘制相邻顶点 */
        WireFrame = 2,
        /* 填充顶点形成的三角形， 不绘制相邻顶点 */
        Solid = 3,
    };

    /* 剔除模式 */
    enum class FGpuPipelineCullMode : uint8_t
    {
        /* 始终绘制所有三角形 */
        Off = 1,
        /* 不要绘制正面的三角形 */
        Front = 2,
        /* 不要绘制朝背的三角形 */
        Back = 3,
    };

    /* 混合类型 */
    enum class FGpuPipelineBlendType : uint8_t
    {
        Zero                = 1,
        One                 = 2,
        SrcColor            = 3,
        InvSrcColor         = 4,
        SrcAlpha            = 5,
        InvSrcAlpha         = 6,
        DstAlpha            = 7,
        InvDstAlpha         = 8,
        DstColor            = 9,
        InvDstColor         = 10,
        SrcAlphaSat         = 11,
        BlendFactor         = 14,
        BlendInvBlendFactor = 15,
        Src1Color           = 16,
        InvSrc1Color        = 17,
        Src1Alpha           = 18,
        InvSrc1Alpha        = 19,
        AlphaFactor         = 20,
        InvAlphaFactor      = 21,
    };

    /* 混合操作 */
    enum class FGpuPipelineBlendOp : uint8_t
    {
        None   = 0,
        Add    = 1,
        Sub    = 2,
        RevSub = 3,
        Min    = 4,
        Max    = 5,
    };

    /* 逻辑操作 */
    enum class FGpuPipelineLogicOp : uint8_t
    {
        None = 0,
        Clear,
        One,
        Copy,
        CopyInv,
        Noop,
        Inv,
        And,
        NAnd,
        Or,
        Nor,
        Xor,
        Equiv,
        AndRev,
        AndInv,
        OrRev,
        OrInv,
    };

    struct FGpuPipelineRtBlendState
    {
        FGpuPipelineSwitch blend;
        FGpuPipelineBlendType src_blend;
        FGpuPipelineBlendType dst_blend;
        FGpuPipelineBlendOp blend_op;
        FGpuPipelineBlendType src_alpha_blend;
        FGpuPipelineBlendType dst_alpha_blend;
        FGpuPipelineBlendOp alpha_blend_op;
        FGpuPipelineLogicOp logic_op;
        FGpuPipelineColorMask write_mask;
    };

    struct FGpuPipelineBlendState
    {
        FGpuPipelineRtBlendState rts[8];
        FGpuPipelineSwitch alpha_to_coverage;
        FGpuPipelineSwitch independent_blend;
    };

    struct FGpuPipelineRasterizerState
    {
        FGpuPipelineFillMode fill_mode;
        FGpuPipelineCullMode cull_mode;
        FGpuPipelineSwitch depth_clip;
        FGpuPipelineSwitch multisample;
        uint32_t forced_sample_count;
        int32_t depth_bias;
        float depth_bias_clamp;
        float slope_scaled_depth_bias;
        FGpuPipelineSwitch aa_line;
        FGpuPipelineSwitch conservative;
    };

    enum class FGpuPipelineDepthWriteMask : uint8_t
    {
        Zero = 0,
        All  = 1,
    };

    enum class FGpuPipelineCmpFunc : uint8_t
    {
        Off          = 0,
        Never        = 1,
        Less         = 2,
        Equal        = 3,
        LessEqual    = 4,
        Greater      = 5,
        NotEqual     = 6,
        GreaterEqual = 7,
        Always       = 8,
    };

    enum class FGpuPipelineStencilFailOp : uint8_t
    {
        Keep    = 1,
        Zero    = 2,
        Replace = 3,
        IncSat  = 4,
        DecSat  = 5,
        Invert  = 6,
        Inc     = 7,
        Dec     = 8,
    };

    struct FGpuPipelineStencilState
    {
        FGpuPipelineStencilFailOp fail_op;
        FGpuPipelineStencilFailOp depth_fail_op;
        FGpuPipelineStencilFailOp pass_op;
        FGpuPipelineCmpFunc func;
    };

    struct FGpuPipelineDepthStencilState
    {
        FGpuPipelineCmpFunc depth_func;
        FGpuPipelineDepthWriteMask depth_write_mask;
        FGpuPipelineSwitch stencil_enable;
        uint8_t stencil_read_mask;
        uint8_t stencil_write_mask;
        FGpuPipelineStencilState front_face;
        FGpuPipelineStencilState back_face;
    };

    struct FGpuPipelineSampleState
    {
        uint32_t count;
        int32_t quality;
    };

    struct FGpuPipelineStateCreateOptions
    {
        FrStr16 name;
        FGpuPipelineCreateFlag flag;
        FGpuPipelineBlendState blend_state;
        FGpuPipelinePrimitiveTopologyType primitive_topology_type;
        FGpuPipelineRasterizerState rasterizer_state;
        FGpuPipelineDepthStencilState depth_stencil_state;
        uint32_t sample_mask;
        int32_t rt_count;
        FrStr8 blob[3];
        FGpuTextureFormat rtv_formats[8];
        FGpuTextureFormat dsv_format;
        FGpuPipelineSampleState sample_state;
    };

    struct FGpuPipelineState : FObject
    {
        // 不获取所有权
        virtual FGpuPipelineLayout* get_layout_ref() const noexcept = 0;

        virtual void* get_raw_ptr() const noexcept = 0;
    };
} // ccc
