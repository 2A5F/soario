﻿using System.Runtime.CompilerServices;
using Coplt.ShaderReflections;
using Soario.Native;

namespace Soario.Rendering;

public sealed class ShaderPass : IDisposable
{
    #region Fields

    public Shader Shader { get; }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Meta.Name;
    }

    public int Index { get; }

    public ShaderPassStage? Ps { get; }
    public ShaderPassStage? Vs { get; }
    public ShaderPassStage? Cs { get; }
    public ShaderPassStage? Ms { get; }
    public ShaderPassStage? Ts { get; }

    public ref readonly MetaData Meta
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref m_meta;
    }

    private MetaData m_meta;

    #endregion

    #region Ctor

    internal ShaderPass(Shader shader, int index, Shader.PassData data)
    {
        Shader = shader;
        Index = index;
        m_meta = data.Meta;
        Ps = data.Ps is { } p ? new(p) : null;
        Vs = data.Vs is { } v ? new(v) : null;
        Cs = data.Cs is { } c ? new(c) : null;
        Ms = data.Ms is { } m ? new(m) : null;
        Ts = data.Ts is { } a ? new(a) : null;
    }

    #endregion

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

    #region Meta

    public record struct MetaData
    {
        public string Name;

        public bool BindLess;

        /// <inheritdoc cref="ShaderFill"/>
        public ShaderFill Fill;
        /// <inheritdoc cref="ShaderCull"/>
        public ShaderCull Cull;
        /// <summary>
        /// 是否启用保守光栅化
        /// </summary>
        public ShaderSwitch Conservative;
        /// <summary>
        /// 深度偏移
        /// </summary>
        public ShaderDepthBias DepthBias;
        /// <summary>
        /// 深度裁剪
        /// </summary>
        public ShaderSwitch ZClip;
        /// <summary>
        /// 深度测试比较方式
        /// </summary>
        public ShaderComp ZTest;
        /// <summary>
        /// 深度写入
        /// </summary>
        public ShaderSwitch ZWrite;
        /// <summary>
        /// 模板
        /// </summary>
        public StencilMeta? Stencil;

        public BlendRts BlendRts;

        public int RtCount;
    }

    [InlineArray(8)]
    public struct BlendRts
    {
        private RtBlendMeta _;
    }

    /// <summary>
    /// 模板
    /// </summary>
    public record struct StencilMeta
    {
        /// <summary>
        /// 参考值
        /// </summary>
        public byte? Ref;
        /// <summary>
        /// 读取遮罩
        /// </summary>
        public byte ReadMask;
        /// <summary>
        /// 写入遮罩
        /// </summary>
        public byte WriteMask;
        /// <summary>
        /// 单独覆盖正面逻辑
        /// </summary>
        public StencilLogicMeta Front;
        /// <summary>
        /// 单独覆盖背面逻辑
        /// </summary>
        public StencilLogicMeta Back;
    }

    /// <summary>
    /// 模板操作逻辑
    /// </summary>
    public record struct StencilLogicMeta
    {
        /// <summary>
        /// 比较方式
        /// </summary>
        public ShaderComp Comp;
        /// <summary>
        /// 通过时的操作
        /// </summary>
        public ShaderStencilOp Pass;
        /// <summary>
        /// 失败时的操作
        /// </summary>
        public ShaderStencilOp Fail;
        /// <summary>
        /// 深度失败时的操作
        /// </summary>
        public ShaderStencilOp ZFail;
    }

    public record struct RtBlendMeta
    {
        /// <summary>
        /// 颜色遮罩 R G B A 任意组合
        /// </summary>
        public ShaderColorMask ColorMask;
        /// <summary>
        /// 混合模式
        /// </summary>
        public ShaderBlend SrcBlend;
        /// <summary>
        /// 混合模式
        /// </summary>
        public ShaderBlend DstBlend;
        /// <summary>
        /// 混合操作 
        /// </summary>
        public ShaderBlendOp BlendOp;
        /// <summary>
        /// 混合模式
        /// </summary>
        public ShaderBlend AlphaSrcBlend;
        /// <summary>
        /// 混合模式
        /// </summary>
        public ShaderBlend AlphaDstBlend;
        /// <summary>
        /// 混合操作 
        /// </summary>
        public ShaderBlendOp AlphaBlendOp;
        /// <summary>
        /// 逻辑操作
        /// </summary>
        public ShaderRtLogicOp? LogicOp;
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

    internal ShaderPassStage(Shader.StageData data)
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
