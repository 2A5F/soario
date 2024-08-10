using Soario.Native;

namespace Soario.Rendering;

/// <summary>
/// 着色器阶段
/// </summary>
public enum ShaderStage
{
    /// <summary>
    /// 像素着色器
    /// </summary>
    Ps = 1,
    /// <summary>
    /// 顶点着色器
    /// </summary>
    Vs,
    /// <summary>
    /// 计算着色器
    /// </summary>
    Cs,
    /// <summary>
    /// 面网着色器
    /// </summary>
    Ms,
    /// <summary>
    /// 任务着色器
    /// </summary>
    Ts,
}

/// <summary>
/// 语义化的 bool 值
/// </summary>
public enum ShaderSwitch
{
    /// <summary>
    /// 关闭
    /// </summary>
    Off = 0,
    /// <summary>
    /// 启用
    /// </summary>
    On = 1,
}

/// <summary>
/// 填充模式
/// </summary>
public enum ShaderFill
{
    /// <summary>
    /// 绘制连接顶点的线条， 不绘制相邻顶点
    /// </summary>
    WireFrame = 2,
    /// <summary>
    /// 填充顶点形成的三角形， 不绘制相邻顶点
    /// </summary>
    Solid = 3,
}

/// <summary>
/// 剔除模式
/// </summary>
public enum ShaderCull
{
    /// <summary>
    /// 始终绘制所有三角形
    /// </summary>
    Off = 1,
    /// <summary>
    /// 不要绘制正面的三角形
    /// </summary>
    Front = 2,
    /// <summary>
    /// 不要绘制朝背的三角形
    /// </summary>
    Back = 3,
}

/// <summary>
/// 比较方式
/// </summary>
public enum ShaderComp
{
    Off = 0,
    Never = 1,
    Lt = 2,
    Eq = 3,
    LE = 4,
    Gt = 5,
    Ne = 6,
    Ge = 7,
    Always = 8,
}

internal static partial class ShaderEx
{
    public static FGpuPipelineCmpFunc ToFFI(this ShaderComp comp) => comp switch
    {
        ShaderComp.Off => FGpuPipelineCmpFunc.Off,
        ShaderComp.Never => FGpuPipelineCmpFunc.Never,
        ShaderComp.Lt => FGpuPipelineCmpFunc.Less,
        ShaderComp.Eq => FGpuPipelineCmpFunc.Equal,
        ShaderComp.LE => FGpuPipelineCmpFunc.LessEqual,
        ShaderComp.Gt => FGpuPipelineCmpFunc.Greater,
        ShaderComp.Ne => FGpuPipelineCmpFunc.NotEqual,
        ShaderComp.Ge => FGpuPipelineCmpFunc.GreaterEqual,
        ShaderComp.Always => FGpuPipelineCmpFunc.Always,
        _ => throw new ArgumentOutOfRangeException(nameof(comp), comp, null)
    };
}

/// <summary>
/// 模板操作
/// </summary>
public enum ShaderStencilOp
{
    Keep = 1,
    Zero = 2,
    Replace = 3,
    IncrSat = 4,
    DecrSat = 5,
    Invert = 6,
    Incr = 7,
    Decr = 8,
}

internal static partial class ShaderEx
{
    public static FGpuPipelineStencilFailOp ToFFI(this ShaderStencilOp op) => op switch
    {
        ShaderStencilOp.Keep => FGpuPipelineStencilFailOp.Keep,
        ShaderStencilOp.Zero => FGpuPipelineStencilFailOp.Zero,
        ShaderStencilOp.Replace => FGpuPipelineStencilFailOp.Replace,
        ShaderStencilOp.IncrSat => FGpuPipelineStencilFailOp.IncSat,
        ShaderStencilOp.DecrSat => FGpuPipelineStencilFailOp.DecSat,
        ShaderStencilOp.Invert => FGpuPipelineStencilFailOp.Invert,
        ShaderStencilOp.Incr => FGpuPipelineStencilFailOp.Inc,
        ShaderStencilOp.Decr => FGpuPipelineStencilFailOp.Dec,
        _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
    };
}

/// <summary>
/// 模板操作逻辑
/// </summary>
public record ShaderStencilLogic
{
    /// <summary>
    /// 比较方式
    /// </summary>
    public ShaderComp? Comp { get; set; }
    /// <summary>
    /// 通过时的操作
    /// </summary>
    public ShaderStencilOp? Pass { get; set; }
    /// <summary>
    /// 失败时的操作
    /// </summary>
    public ShaderStencilOp? Fail { get; set; }
    /// <summary>
    /// 深度失败时的操作
    /// </summary>
    public ShaderStencilOp? ZFail { get; set; }
}

/// <summary>
/// 模板
/// </summary>
public record ShaderStencil : ShaderStencilLogic
{
    /// <summary>
    /// 参考值
    /// </summary>
    public byte? Ref { get; set; }
    /// <summary>
    /// 读取遮罩
    /// </summary>
    public byte? ReadMask { get; set; }
    /// <summary>
    /// 写入遮罩
    /// </summary>
    public byte? WriteMask { get; set; }
    /// <summary>
    /// 单独覆盖正面逻辑
    /// </summary>
    public ShaderStencilLogic? Front { get; set; }
    /// <summary>
    /// 单独覆盖背面逻辑
    /// </summary>
    public ShaderStencilLogic? Back { get; set; }
}

public enum ShaderRtLogicOp : byte
{
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
}

internal static partial class ShaderEx
{
    public static FGpuPipelineLogicOp ToFFI(this ShaderRtLogicOp blend) => blend switch
    {
        ShaderRtLogicOp.Clear => FGpuPipelineLogicOp.Clear,
        ShaderRtLogicOp.One => FGpuPipelineLogicOp.One,
        ShaderRtLogicOp.Copy => FGpuPipelineLogicOp.Copy,
        ShaderRtLogicOp.CopyInv => FGpuPipelineLogicOp.CopyInv,
        ShaderRtLogicOp.Noop => FGpuPipelineLogicOp.Noop,
        ShaderRtLogicOp.Inv => FGpuPipelineLogicOp.Inv,
        ShaderRtLogicOp.And => FGpuPipelineLogicOp.And,
        ShaderRtLogicOp.NAnd => FGpuPipelineLogicOp.NAnd,
        ShaderRtLogicOp.Or => FGpuPipelineLogicOp.Or,
        ShaderRtLogicOp.Nor => FGpuPipelineLogicOp.Nor,
        ShaderRtLogicOp.Xor => FGpuPipelineLogicOp.Xor,
        ShaderRtLogicOp.Equiv => FGpuPipelineLogicOp.Equiv,
        ShaderRtLogicOp.AndRev => FGpuPipelineLogicOp.AndRev,
        ShaderRtLogicOp.AndInv => FGpuPipelineLogicOp.AndInv,
        ShaderRtLogicOp.OrRev => FGpuPipelineLogicOp.OrRev,
        ShaderRtLogicOp.OrInv => FGpuPipelineLogicOp.OrInv,
        _ => throw new ArgumentOutOfRangeException(nameof(blend), blend, null)
    };
}

public enum ShaderBlend : byte
{
    Zero = 1,
    One = 2,
    SrcColor = 3,
    InvSrcColor = 4,
    SrcAlpha = 5,
    InvSrcAlpha = 6,
    DstAlpha = 7,
    InvDstAlpha = 8,
    DstColor = 9,
    InvDstColor = 10,
    SrcAlphaSat = 11,
    BlendFactor = 14,
    BlendInvBlendFactor = 15,
    Src1Color = 16,
    InvSrc1Color = 17,
    Src1Alpha = 18,
    InvSrc1Alpha = 19,
    AlphaFactor = 20,
    InvAlphaFactor = 21,
}

internal static partial class ShaderEx
{
    public static FGpuPipelineBlendType ToFFI(this ShaderBlend blend) => blend switch
    {
        ShaderBlend.Zero => FGpuPipelineBlendType.Zero,
        ShaderBlend.One => FGpuPipelineBlendType.One,
        ShaderBlend.SrcColor => FGpuPipelineBlendType.SrcColor,
        ShaderBlend.InvSrcColor => FGpuPipelineBlendType.InvSrcColor,
        ShaderBlend.SrcAlpha => FGpuPipelineBlendType.SrcAlpha,
        ShaderBlend.InvSrcAlpha => FGpuPipelineBlendType.InvSrcAlpha,
        ShaderBlend.DstAlpha => FGpuPipelineBlendType.DstAlpha,
        ShaderBlend.InvDstAlpha => FGpuPipelineBlendType.InvDstAlpha,
        ShaderBlend.DstColor => FGpuPipelineBlendType.DstColor,
        ShaderBlend.InvDstColor => FGpuPipelineBlendType.InvDstColor,
        ShaderBlend.SrcAlphaSat => FGpuPipelineBlendType.SrcAlphaSat,
        ShaderBlend.BlendFactor => FGpuPipelineBlendType.BlendFactor,
        ShaderBlend.BlendInvBlendFactor => FGpuPipelineBlendType.BlendInvBlendFactor,
        ShaderBlend.Src1Color => FGpuPipelineBlendType.Src1Color,
        ShaderBlend.InvSrc1Color => FGpuPipelineBlendType.InvSrc1Color,
        ShaderBlend.Src1Alpha => FGpuPipelineBlendType.Src1Alpha,
        ShaderBlend.InvSrc1Alpha => FGpuPipelineBlendType.InvSrc1Alpha,
        ShaderBlend.AlphaFactor => FGpuPipelineBlendType.AlphaFactor,
        ShaderBlend.InvAlphaFactor => FGpuPipelineBlendType.InvAlphaFactor,
        _ => throw new ArgumentOutOfRangeException(nameof(blend), blend, null)
    };
}

public enum ShaderBlendOp : byte
{
    Off = 0,
    Add = 1,
    Sub = 2,
    RevSub = 3,
    Min = 4,
    Max = 5,
}

internal static partial class ShaderEx
{
    public static FGpuPipelineBlendOp ToFFI(this ShaderBlendOp op) => op switch
    {
        ShaderBlendOp.Off => FGpuPipelineBlendOp.None,
        ShaderBlendOp.Add => FGpuPipelineBlendOp.Add,
        ShaderBlendOp.Sub => FGpuPipelineBlendOp.Sub,
        ShaderBlendOp.RevSub => FGpuPipelineBlendOp.RevSub,
        ShaderBlendOp.Min => FGpuPipelineBlendOp.Min,
        ShaderBlendOp.Max => FGpuPipelineBlendOp.Max,
        _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
    };
}

public record ShaderPassRtDesc
{
    /// <summary>
    /// 颜色遮罩 R G B A 任意组合
    /// </summary>
    public ShaderColorMask? ColorMask { get; set; }
    /// <summary>
    /// 混合模式 
    /// </summary>
    public ShaderBlendGroup? Blend { get; set; }
    /// <summary>
    /// 混合操作
    /// </summary>
    public ShaderBlendOpGroup? BlendOp { get; set; }
    /// <summary>
    /// 逻辑操作
    /// </summary>
    public ShaderRtLogicOp? LogicOp { get; set; }
}

/// <summary>
/// 此类型仅用于反序列化
/// </summary>
public record ShaderPassStateMeta : ShaderPassRtDesc
{
    /// <inheritdoc cref="ShaderFill"/>
    public ShaderFill? Fill { get; set; }
    /// <inheritdoc cref="ShaderCull"/>
    public ShaderCull? Cull { get; set; }
    /// <summary>
    /// 是否启用保守光栅化
    /// </summary>
    public ShaderSwitch? Conservative { get; set; }
    /// <inheritdoc cref="ShaderDepthBias"/>
    public ShaderDepthBias? DepthBias { get; set; }
    /// <summary>
    /// 深度裁剪
    /// </summary>
    public ShaderSwitch? ZClip { get; set; }
    /// <summary>
    /// 深度测试比较方式
    /// </summary>
    public ShaderComp? ZTest { get; set; }
    /// <summary>
    /// 深度写入
    /// </summary>
    public ShaderSwitch? ZWrite { get; set; }
    /// <summary>
    /// 模板
    /// </summary>
    public ShaderStencil? Stencil { get; set; }

    public ShaderPassRtDesc? Rt0 { get; set; }
    public ShaderPassRtDesc? Rt1 { get; set; }
    public ShaderPassRtDesc? Rt2 { get; set; }
    public ShaderPassRtDesc? Rt3 { get; set; }
    public ShaderPassRtDesc? Rt4 { get; set; }
    public ShaderPassRtDesc? Rt5 { get; set; }
    public ShaderPassRtDesc? Rt6 { get; set; }
    public ShaderPassRtDesc? Rt7 { get; set; }
}

/// <summary>
/// 着色器步骤元信息，此类型仅用于反序列化
/// </summary>
public record ShaderPassMeta : ShaderPassStateMeta
{
    /// <summary>
    /// 着色器阶段
    /// </summary>
    public required List<string> Stages { get; set; }
}

/// <summary>
/// 着色器元信息，此类型仅用于反序列化
/// </summary>
public record ShaderAssetMeta : ShaderPassStateMeta
{
    /// <summary>
    /// 资产 ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 资产路径
    /// </summary>
    public required string Path { get; set; }
    /// <summary>
    /// 着色器步骤
    /// </summary>
    public required Dictionary<string, ShaderPassMeta> Pass { get; set; }
}

/// <summary>
/// 语法：深度值 , 深度斜率 , 深度偏移 // todo json parser
/// </summary>
public record struct ShaderDepthBias
{
    /// <summary>
    /// 添加到给定像素的深度值
    /// </summary>
    public int DepthBias { get; set; }
    /// <summary>
    /// 像素的最大深度偏差
    /// </summary>
    public float DepthBiasClamp { get; set; }
    /// <summary>
    /// 给定像素斜率上的标量
    /// </summary>
    public float SlopeScaledDepthBias { get; set; }
}

/// <summary>
/// 颜色遮罩 R G B A 任意组合 // todo 搞个专用结构处理 json 序列化
/// </summary>
public record struct ShaderColorMask
{
    public bool R { get; set; }
    public bool G { get; set; }
    public bool B { get; set; }
    public bool A { get; set; }

    public static readonly ShaderColorMask All = new() { R = true, G = true, B = true, A = true };

    internal FGpuPipelineColorMask ToFFI()
    {
        var r = new FGpuPipelineColorMask();
        if (R) r.r = 1;
        if (G) r.g = 1;
        if (B) r.b = 1;
        if (A) r.a = 1;
        return r;
    }
}

public record struct ShaderBlendGroup // todo 搞个专用结构处理 json 序列化
{
    public ShaderBlend SrcBlend { get; set; }
    public ShaderBlend DstBlend { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend AlphaSrcBlend { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend AlphaDstBlend { get; set; }
}

public record struct ShaderBlendOpGroup // todo 搞个专用结构处理 json 序列化
{
    /// <summary>
    /// 混合操作 
    /// </summary>
    public ShaderBlendOp BlendOp { get; set; }
    /// <summary>
    /// 混合操作 
    /// </summary>
    public ShaderBlendOp AlphaBlendOp { get; set; }
}
