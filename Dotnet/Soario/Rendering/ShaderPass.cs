using System.Runtime.CompilerServices;
using Coplt.ShaderReflections;
using Soario.Native;

namespace Soario.Rendering;

public sealed class ShaderPass : IDisposable
{
    #region Fields

    public Shader Shader { get; }

    public string Name { get; }

    public int Index { get; }

    public bool BindLess { get; } = true;

    public ShaderPassStage? Ps { get; }
    public ShaderPassStage? Vs { get; }
    public ShaderPassStage? Cs { get; }
    public ShaderPassStage? Ms { get; }
    public ShaderPassStage? Ts { get; }

    /// <inheritdoc cref="ShaderFill"/>
    public ShaderFill Fill { get; set; }
    /// <inheritdoc cref="ShaderCull"/>
    public ShaderCull Cull { get; set; }
    /// <summary>
    /// 是否启用保守光栅化
    /// </summary>
    public ShaderSwitch Conservative { get; set; }
    /// <summary>
    /// 深度偏移
    /// </summary>
    public ShaderDepthBias DepthBias { get; set; }
    /// <summary>
    /// 深度裁剪
    /// </summary>
    public ShaderSwitch ZClip { get; set; }
    /// <summary>
    /// 深度测试比较方式
    /// </summary>
    public ShaderComp ZTest { get; set; }
    /// <summary>
    /// 深度写入
    /// </summary>
    public ShaderSwitch ZWrite { get; set; }
    /// <summary>
    /// 模板
    /// </summary>
    public ShaderStencilData? Stencil { get; set; }

    public Shader.BlendRts BlendRtsRts { get; set; }
        
    public int RtCount { get; set; }
    
    #endregion

    public ShaderPass(Shader shader, int index, Shader.PassData data)
    {
        Shader = shader;
        Index = index;
        Name = data.Name;
        BindLess = data.BindLess;
        Ps = data.Ps is { } p ? new(p) : null;
        Vs = data.Vs is { } v ? new(v) : null;
        Cs = data.Cs is { } c ? new(c) : null;
        Ms = data.Ms is { } m ? new(m) : null;
        Ts = data.Ts is { } a ? new(a) : null;

        Fill = data.Fill;
        Cull = data.Cull;
        Conservative = data.Conservative;
        DepthBias = data.DepthBias;
        ZClip = data.ZClip;
        ZTest = data.ZTest;
        ZWrite = data.ZWrite;
        Stencil = data.Stencil;
    }

    #region Dispose

    public void Dispose()
    {
        Ps?.Dispose();
        Vs?.Dispose();
        Cs?.Dispose();
        Ms?.Dispose();
        Ts?.Dispose();
    }

    #endregion
}

public sealed class ShaderPassStage : IDisposable
{
    #region Fields

    public ShaderStage ShaderStage { get; }
    public ShaderMeta Reflection { get; }
    private Blob blob;

    public ref readonly Blob Blob
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref blob;
    }

    #endregion

    #region Ctor

    public ShaderPassStage(Shader.StageData data)
    {
        blob = new(data.Blob);
        Reflection = data.Reflection;
        ShaderStage = data.ShaderStage;
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (!blob.IsNull)
        {
            blob.Dispose();
            blob = default;
        }
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    ~ShaderPassStage() => ReleaseUnmanagedResources();

    #endregion
}

/// <summary>
/// 模板
/// </summary>
public record struct ShaderStencilData
{
    /// <summary>
    /// 参考值
    /// </summary>
    public byte? Ref { get; set; }
    /// <summary>
    /// 读取遮罩
    /// </summary>
    public byte ReadMask { get; set; }
    /// <summary>
    /// 写入遮罩
    /// </summary>
    public byte WriteMask { get; set; }
    /// <summary>
    /// 单独覆盖正面逻辑
    /// </summary>
    public ShaderStencilLogicData Front { get; set; }
    /// <summary>
    /// 单独覆盖背面逻辑
    /// </summary>
    public ShaderStencilLogicData Back { get; set; }
}

/// <summary>
/// 模板操作逻辑
/// </summary>
public record struct ShaderStencilLogicData
{
    /// <summary>
    /// 比较方式
    /// </summary>
    public ShaderComp Comp { get; set; }
    /// <summary>
    /// 通过时的操作
    /// </summary>
    public ShaderStencilOp Pass { get; set; }
    /// <summary>
    /// 失败时的操作
    /// </summary>
    public ShaderStencilOp Fail { get; set; }
    /// <summary>
    /// 深度失败时的操作
    /// </summary>
    public ShaderStencilOp ZFail { get; set; }
}

public record struct ShaderPassRtBlendData
{
    /// <summary>
    /// 颜色遮罩 R G B A 任意组合
    /// </summary>
    public ShaderColorMask ColorMask { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend SrcBlend { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend DstBlend { get; set; }
    /// <summary>
    /// 混合操作 
    /// </summary>
    public ShaderBlendOp BlendOp { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend AlphaSrcBlend { get; set; }
    /// <summary>
    /// 混合模式
    /// </summary>
    public ShaderBlend AlphaDstBlend { get; set; }
    /// <summary>
    /// 混合操作 
    /// </summary>
    public ShaderBlendOp AlphaBlendOp { get; set; }
    /// <summary>
    /// 逻辑操作
    /// </summary>
    public ShaderRtLogicOp? LogicOp { get; set; }
}
