using System.Runtime.CompilerServices;
using System.Text;
using Soario.Native;

namespace Soario.Rendering;

public ref struct GpuPipelineStateCreateOptions
{
    public string? Name { get; set; }
    public ShaderPass Pass { get; set; }
    public GpuTextureFormat? RtFormat { get; set; }
    public ReadOnlySpan<GpuTextureFormat> RtFormats { get; set; }
    public GpuDepthFormat DepthFormat { get; set; }

    #region Ctor

    public GpuPipelineStateCreateOptions(ShaderPass pass,
        GpuTextureFormat rt_format,
        GpuDepthFormat depth_format = GpuDepthFormat.Unknown
    ) : this(pass, rt_format, [], depth_format) { }

    public GpuPipelineStateCreateOptions(ShaderPass pass,
        ReadOnlySpan<GpuTextureFormat> rt_formats,
        GpuDepthFormat depth_format = GpuDepthFormat.Unknown
    ) : this(pass, null, rt_formats, depth_format) { }

    private GpuPipelineStateCreateOptions(
        ShaderPass pass,
        GpuTextureFormat? base_rt_format,
        ReadOnlySpan<GpuTextureFormat> rt_formats,
        GpuDepthFormat depth_format = GpuDepthFormat.Unknown
    )
    {
        RtFormat = base_rt_format;
        var sb = new StringBuilder();
        var first = true;
        if (rt_formats.Length != pass.Meta.RtCount && base_rt_format is null)
            throw new ArgumentException("Rt count not same");
        if (base_rt_format is { } format)
        {
            for (var i = 0; i < pass.Meta.RtCount; i++)
            {
                if (first) first = false;
                else sb.Append(", ");
                sb.Append($"{format}");
            }
        }
        else
        {
            foreach (var rt_format in RtFormats)
            {
                if (first) first = false;
                else sb.Append(", ");
                sb.Append($"{rt_format}");
            }
        }
        Name =
            $"Shader [{pass.Shader.Path ?? $"{pass.Shader.Id:D}"}] Pass ({pass.Index} : {pass.Name}) {{ rtv = [{sb}]; dsv = {depth_format} }}";
        Pass = pass;
        RtFormats = rt_formats;
        DepthFormat = depth_format;
    }

    #endregion
}

public sealed unsafe class GpuPipelineState : IDisposable
{
    #region Fields

    internal readonly GpuDevice m_device;
    internal readonly GpuPipelineLayout m_layout;
    internal readonly ShaderPass m_pass;
    internal FGpuPipelineState* m_inner;
    internal readonly string m_name;

    #endregion

    #region Props

    public GpuDevice Device
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_device;
    }

    public GpuPipelineLayout Layout
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_layout;
    }

    public ShaderPass Pass
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_pass;
    }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_name;
    }

    #endregion

    #region Ctor

    internal GpuPipelineState(GpuDevice device, GpuPipelineLayout layout, GpuPipelineStateCreateOptions options)
    {
        m_device = device;
        m_layout = layout;
        m_pass = options.Pass;
        m_name = options.Name ?? $"Anonymous Pipeline State ({Guid.NewGuid():D})";
        fixed (char* p_name = m_name)
        {
            var pass = options.Pass;

            var f_options = new FGpuPipelineStateCreateOptions
            {
                name = new() { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
                primitive_topology_type = FGpuPipelinePrimitiveTopologyType.Triangle,
                rt_count = pass.Meta.RtCount,
                sample_state = new()
                {
                    count = 1,
                    quality = 0,
                }
            };

            #region InitFlag And Blob

            var flag = new FGpuPipelineCreateFlag();
            if (pass.Meta.BindLess) flag.bind_less = 1;
            if (pass.Cs is { Blob.UnsafeSlice: var cs })
            {
                flag.cs = 1;
                f_options.blob[0] = cs;
            }
            else if (pass.Ps is { Blob.UnsafeSlice: var ps })
            {
                flag.ps = 1;
                f_options.blob[0] = ps;
                if (pass.Vs is { Blob.UnsafeSlice: var vs })
                {
                    flag.vs = 1;
                    f_options.blob[1] = vs;
                }
                else if (pass.Ms is { Blob.UnsafeSlice: var ms })
                {
                    flag.ms = 1;
                    f_options.blob[1] = ms;
                    if (pass.Ts is { Blob.UnsafeSlice: var ts })
                    {
                        flag.ts = 1;
                        f_options.blob[2] = ts;
                    }
                }
                else throw new ArgumentException("This shader combination is not supported");
            }
            else throw new ArgumentException("This shader combination is not supported");

            f_options.flag = flag;

            #endregion

            #region Rasterizer State

            f_options.rasterizer_state = new()
            {
                fill_mode = pass.Meta.Fill switch
                {
                    ShaderFill.WireFrame => FGpuPipelineFillMode.WireFrame,
                    ShaderFill.Solid => FGpuPipelineFillMode.Solid,
                    _ => throw new ArgumentOutOfRangeException()
                },
                cull_mode = pass.Meta.Cull switch
                {
                    ShaderCull.Off => FGpuPipelineCullMode.Off,
                    ShaderCull.Front => FGpuPipelineCullMode.Front,
                    ShaderCull.Back => FGpuPipelineCullMode.Back,
                    _ => throw new ArgumentOutOfRangeException()
                },
                depth_clip = pass.Meta.ZClip is ShaderSwitch.On ? FGpuPipelineSwitch.On : FGpuPipelineSwitch.Off,
                multisample = FGpuPipelineSwitch.Off,
                forced_sample_count = 0,
                depth_bias = pass.Meta.DepthBias.DepthBias,
                depth_bias_clamp = pass.Meta.DepthBias.DepthBiasClamp,
                slope_scaled_depth_bias = pass.Meta.DepthBias.SlopeScaledDepthBias,
                aa_line = FGpuPipelineSwitch.Off,
                conservative = pass.Meta.Conservative is ShaderSwitch.On
                    ? FGpuPipelineSwitch.On
                    : FGpuPipelineSwitch.Off,
            };

            #endregion

            #region Depth Stencil State

            if (options.DepthFormat is not GpuDepthFormat.Unknown)
            {
                f_options.depth_stencil_state = new()
                {
                    depth_func = pass.Meta.ZWrite is ShaderSwitch.Off
                        ? FGpuPipelineCmpFunc.Off
                        : pass.Meta.ZTest.ToFFI(),
                    depth_write_mask = FGpuPipelineDepthWriteMask.All,
                };
                if (pass.Meta.Stencil is { } stencil)
                {
                    ref var s = ref f_options.depth_stencil_state;
                    s.stencil_enable = FGpuPipelineSwitch.On;
                    s.stencil_read_mask = stencil.ReadMask;
                    s.stencil_write_mask = stencil.WriteMask;
                    s.front_face = new()
                    {
                        func = stencil.Front.Comp.ToFFI(),
                        fail_op = stencil.Front.Fail.ToFFI(),
                        pass_op = stencil.Front.Pass.ToFFI(),
                        depth_fail_op = stencil.Front.ZFail.ToFFI(),
                    };
                    s.back_face = new()
                    {
                        func = stencil.Back.Comp.ToFFI(),
                        fail_op = stencil.Back.Fail.ToFFI(),
                        pass_op = stencil.Back.Pass.ToFFI(),
                        depth_fail_op = stencil.Back.ZFail.ToFFI(),
                    };
                }
            }

            #endregion

            #region Blends

            f_options.blend_state = new FGpuPipelineBlendState
            {
                independent_blend = pass.Meta.RtCount > 1 && options.RtFormat is null
                    ? FGpuPipelineSwitch.On
                    : FGpuPipelineSwitch.Off,
                alpha_to_coverage = FGpuPipelineSwitch.Off,
            };

            for (var i = 0; i < pass.Meta.RtCount; i++)
            {
                var src = pass.Meta.BlendRts[i];
                ref var dst = ref f_options.blend_state.rts[i];
                dst.blend = src.BlendOp is ShaderBlendOp.Off ? FGpuPipelineSwitch.Off : FGpuPipelineSwitch.On;
                if (dst.blend is FGpuPipelineSwitch.On)
                {
                    dst.blend_op = src.BlendOp.ToFFI();
                    dst.src_blend = src.SrcBlend.ToFFI();
                    dst.dst_blend = src.DstBlend.ToFFI();
                    dst.alpha_blend_op = src.AlphaBlendOp.ToFFI();
                    dst.src_alpha_blend = src.AlphaSrcBlend.ToFFI();
                    dst.dst_alpha_blend = src.AlphaDstBlend.ToFFI();
                    dst.logic_op = src.LogicOp is { } logic_op ? logic_op.ToFFI() : FGpuPipelineLogicOp.None;
                    dst.write_mask = src.ColorMask.ToFFI();
                }
            }

            #endregion

            #region Set Format

            if (options.RtFormat is null)
            {
                for (var i = 0; i < options.RtFormats.Length; i++)
                {
                    var rt_format = options.RtFormats[i];
                    f_options.rtv_formats[i] = rt_format.ToFFI();
                }
            }
            else
            {
                for (var i = 0; i < pass.Meta.RtCount; i++)
                {
                    f_options.rtv_formats[i] = options.RtFormat.Value.ToFFI();
                }
            }
            f_options.dsv_format = options.DepthFormat.ToFFI();

            #endregion

            FError err;
            m_inner = m_device.m_inner->CreatePipelineState(layout.m_inner, &f_options, &err);
            if (m_inner == null) err.Throw();
        }
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    ~GpuPipelineState() => Dispose();

    #endregion

    #region ToString

    public override string ToString() => $"GpuPipelineState({m_name})";

    #endregion
}
