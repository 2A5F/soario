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
        m_passes = passes.Select(static p => new ShaderPass(p)).ToList();
        m_name_2_index = m_passes
            .Select(static (p, i) => (p.Name, i))
            .ToFrozenDictionary(static a => a.Name, a => a.i);
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
        public StageData? Ps { get; set; }
        public StageData? Vs { get; set; }
        public StageData? Cs { get; set; }
        public StageData? Ms { get; set; }
        public StageData? As { get; set; }
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

                var pa = new PassData { Name = name };
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
                        case ShaderStage.As:
                            pa.As = stage;
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
