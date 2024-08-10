using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Coplt.ShaderReflections;
using Soario.Resources;
using ZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;

namespace Soario.Rendering;

public sealed class Shader : AAsset, IEquatable<Shader>
{
    #region Static

    #region PropertyToID

    private static int s_prop_id_inc;
    private static readonly ConcurrentDictionary<string, int> s_prop_to_id_map = new();

    public static int PropertyToID(string name) =>
        s_prop_to_id_map.GetOrAdd(name, static _ => Interlocked.Increment(ref s_prop_id_inc));

    #endregion

    #endregion

    #region Fields

    private readonly FrozenDictionary<string, int> m_name_2_index;
    private readonly List<ShaderPass> m_passes;

    #endregion

    #region Props

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetPassIndex(string name) => m_name_2_index.GetValueOrDefault(name, -1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ShaderPass GetPass(int index) => m_passes[index];

    public ReadOnlySpan<ShaderPass> Passes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => CollectionsMarshal.AsSpan(m_passes);
    }

    #endregion

    #region Ctor

    internal Shader(Guid id, string? path, List<PassData> passes) : base(id, path)
    {
        m_passes = passes.Select((p, i) => new ShaderPass(this, i, p)).ToList();
        m_name_2_index = m_passes
            .ToFrozenDictionary(static a => a.Name, static a => a.Index);
    }

    #endregion

    #region Equals

    public bool Equals(Shader? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Shader other && Equals(other);
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Shader? left, Shader? right) => Equals(left, right);
    public static bool operator !=(Shader? left, Shader? right) => !Equals(left, right);

    #endregion

    #region ToString

    public override string ToString()
    {
        return $"Shader({Path})";
    }

    #endregion

    #region Pass

    public record struct PassData
    {
        public string Name { get; set; }
        public bool BindLess { get; set; }

        public StageData? Ps { get; set; }
        public StageData? Vs { get; set; }
        public StageData? Cs { get; set; }
        public StageData? Ms { get; set; }
        public StageData? Ts { get; set; }

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

        public BlendRts BlendRts { get; set; }

        public int RtCount { get; set; }
    }

    [InlineArray(8)]
    public struct BlendRts
    {
        private ShaderPassRtBlendData _;
    }

    #endregion

    #region Stage

    public readonly struct StageData
    {
        #region Prop

        public byte[] Blob { get; }
        public ShaderMeta Reflection { get; }
        public ShaderStage ShaderStage { get; }

        #endregion

        #region Ctor

        internal StageData(ShaderStage shader_stage, byte[] blob, ShaderMeta reflection)
        {
            ShaderStage = shader_stage;
            Blob = blob;
            Reflection = reflection;
        }

        #endregion
    }

    #endregion

    #region Load

    private static readonly JsonSerializerOptions s_json_serializer_options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static async Task<Shader> Load(string path)
    {
        using var zip = new ZipFile(path);
        var meta_file = zip.GetEntry(".meta")!;
        await using var meta_stream = zip.GetInputStream(meta_file);
        var meta = await JsonSerializer.DeserializeAsync<ShaderAssetMeta>(meta_stream, s_json_serializer_options);
        var passes = new List<PassData>();
        foreach (var (name, pass) in meta!.Pass)
        {
            try
            {
                var stages = await Task.WhenAll(pass.Stages.Select(async stage =>
                {
                    var shader_stage = Enum.Parse<ShaderStage>(stage, ignoreCase: true);

                    // ReSharper disable once AccessToDisposedClosure
                    var obj_file = zip.GetEntry($"{name}.{stage}.o")!;
                    // ReSharper disable once AccessToDisposedClosure
                    var reflection_file = zip.GetEntry($"{name}.{stage}.re")!;

                    var blob = GC.AllocateUninitializedArray<byte>((int)obj_file.Size);

                    // ReSharper disable once AccessToDisposedClosure
                    await using var obj_stream = zip.GetInputStream(obj_file);
                    await obj_stream.ReadExactlyAsync(blob);

                    // ReSharper disable once AccessToDisposedClosure
                    await using var reflection_stream = zip.GetInputStream(reflection_file);
                    var reflection =
                        await JsonSerializer.DeserializeAsync<ShaderMeta>(reflection_stream, s_json_serializer_options);

                    return new StageData(shader_stage, blob, reflection!);
                }));

                var stencil = pass.Stencil ?? meta.Stencil;
                var pa = new PassData
                {
                    Name = name,
                    Fill = pass.Fill ?? meta.Fill ?? ShaderFill.Solid,
                    Cull = pass.Cull ?? meta.Cull ?? ShaderCull.Back,
                    Conservative = pass.Conservative ?? meta.Conservative ?? ShaderSwitch.Off,
                    DepthBias = pass.DepthBias ?? meta.DepthBias ?? default,
                    ZClip = pass.ZClip ?? meta.ZClip ?? ShaderSwitch.On,
                    ZWrite = pass.ZWrite ?? meta.ZWrite ?? ShaderSwitch.On,
                    ZTest = pass.ZTest ?? meta.ZTest ?? ShaderComp.LE,
                    Stencil = stencil is null
                        ? null
                        : new ShaderStencilData
                        {
                            Ref = stencil.Ref,
                            ReadMask = stencil.ReadMask ?? byte.MaxValue,
                            WriteMask = stencil.WriteMask ?? byte.MaxValue,
                            Front = new ShaderStencilLogicData
                            {
                                Comp = stencil.Front?.Comp ?? stencil.Comp ?? ShaderComp.Always,
                                Pass = stencil.Front?.Pass ?? stencil.Pass ?? ShaderStencilOp.Keep,
                                Fail = stencil.Front?.Fail ?? stencil.Fail ?? ShaderStencilOp.Keep,
                                ZFail = stencil.Front?.ZFail ?? stencil.ZFail ?? ShaderStencilOp.Keep,
                            },
                            Back = new ShaderStencilLogicData
                            {
                                Comp = stencil.Back?.Comp ?? stencil.Comp ?? ShaderComp.Always,
                                Pass = stencil.Back?.Pass ?? stencil.Pass ?? ShaderStencilOp.Keep,
                                Fail = stencil.Back?.Fail ?? stencil.Fail ?? ShaderStencilOp.Keep,
                                ZFail = stencil.Back?.ZFail ?? stencil.ZFail ?? ShaderStencilOp.Keep,
                            },
                        },
                };

                #region BlendRts

                var rt_count = 0;
                var rts = new BlendRts();

                static ShaderPassRtBlendData make_rt_blend_data(ShaderPassRtDesc? pass, ShaderPassRtDesc? meta) =>
                    new()
                    {
                        ColorMask = pass?.ColorMask ?? meta?.ColorMask ?? ShaderColorMask.All,
                        BlendOp = pass?.BlendOp?.BlendOp ?? meta?.BlendOp?.BlendOp ?? ShaderBlendOp.Off,
                        SrcBlend = pass?.Blend?.SrcBlend ?? meta?.Blend?.SrcBlend ?? ShaderBlend.One,
                        DstBlend = pass?.Blend?.DstBlend ?? meta?.Blend?.DstBlend ?? ShaderBlend.Zero,
                        AlphaBlendOp = pass?.BlendOp?.AlphaBlendOp ??
                                       meta?.BlendOp?.AlphaBlendOp ?? ShaderBlendOp.Off,
                        AlphaSrcBlend = pass?.Blend?.AlphaSrcBlend ??
                                        meta?.Blend?.AlphaSrcBlend ?? ShaderBlend.One,
                        AlphaDstBlend = pass?.Blend?.AlphaDstBlend ??
                                        meta?.Blend?.AlphaDstBlend ?? ShaderBlend.Zero,
                        LogicOp = pass?.LogicOp ?? meta?.LogicOp ?? ShaderRtLogicOp.Noop,
                    };

                if (pass.Rt0 is { } || meta.Rt0 is { })
                {
                    rt_count = 1;
                    rts[0] = make_rt_blend_data(pass.Rt0, meta.Rt0);

                    if (pass.Rt1 is { } || meta.Rt1 is { })
                    {
                        rt_count = 2;
                        rts[1] = make_rt_blend_data(pass.Rt1, meta.Rt1);

                        if (pass.Rt2 is { } || meta.Rt2 is { })
                        {
                            rt_count = 3;
                            rts[2] = make_rt_blend_data(pass.Rt2, meta.Rt2);

                            if (pass.Rt3 is { } || meta.Rt3 is { })
                            {
                                rt_count = 4;
                                rts[3] = make_rt_blend_data(pass.Rt3, meta.Rt3);

                                if (pass.Rt4 is { } || meta.Rt4 is { })
                                {
                                    rt_count = 5;
                                    rts[4] = make_rt_blend_data(pass.Rt4, meta.Rt4);

                                    if (pass.Rt5 is { } || meta.Rt5 is { })
                                    {
                                        rt_count = 6;
                                        rts[5] = make_rt_blend_data(pass.Rt5, meta.Rt5);

                                        if (pass.Rt6 is { } || meta.Rt6 is { })
                                        {
                                            rt_count = 7;
                                            rts[6] = make_rt_blend_data(pass.Rt6, meta.Rt6);

                                            if (pass.Rt7 is { } || meta.Rt7 is { })
                                            {
                                                rt_count = 8;
                                                rts[7] = make_rt_blend_data(pass.Rt7, meta.Rt7);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                pa.RtCount = rt_count;
                pa.BlendRts = rts;

                #endregion

                foreach (var stage in stages)
                {
                    switch (stage.ShaderStage)
                    {
                        case ShaderStage.Ps:
                            pa.Ps = stage;
                            break;
                        case ShaderStage.Vs:
                            pa.Vs = stage;
                            break;
                        case ShaderStage.Cs:
                            pa.Cs = stage;
                            break;
                        case ShaderStage.Ms:
                            pa.Ms = stage;
                            break;
                        case ShaderStage.Ts:
                            pa.Ts = stage;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                passes.Add(pa);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Throw(e);
            }
        }
        return new(meta.Id, meta.Path, passes);
    }

    #endregion
}
