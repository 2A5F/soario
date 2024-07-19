using System.Buffers;
using System.Collections.Frozen;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Coplt.Mathematics;
using Soario.Native;
using Soario.Resources;

namespace Soario.Rendering;

public sealed class Shader : AAsset, IEquatable<Shader>
{
    #region Fields

    private readonly FrozenDictionary<string, int> m_name_2_index;
    private readonly List<ShaderPass> m_passes;

    #endregion

    #region Props

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetPassIndex(string name) => m_name_2_index.GetValueOrDefault(name, -1);

    #endregion

    #region Ctor

    internal Shader(Guid id, string? path, List<PassData> passes) : base(id, path)
    {
        m_passes = passes.Select(ShaderPass.Load).ToList();
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
        public byte[] ReflectionBlob { get; }
        public ShaderStage ShaderStage { get; }

        #endregion

        #region Ctor

        internal StageData(ShaderStage shader_stage, byte[] blob, byte[] reflection_blob)
        {
            ShaderStage = shader_stage;
            Blob = blob;
            ReflectionBlob = reflection_blob;
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
        using var zip = ZipFile.OpenRead(path);
        var meta_file = zip.GetEntry("meta.json")!;
        await using var meta_stream = meta_file.Open();
        var meta = await JsonSerializer.DeserializeAsync<ShaderMeta>(meta_stream, s_json_serializer_options);
        var passes = new List<PassData>();
        foreach (var (name, pass) in meta.Pass)
        {
            var stages = await Task.WhenAll(pass.Stages.Select(async stage =>
            {
                var shader_stage = Enum.Parse<ShaderStage>(stage, ignoreCase: true);

                // ReSharper disable once AccessToDisposedClosure
                var obj_file = zip.GetEntry($"{name}.{stage}.o")!;
                // ReSharper disable once AccessToDisposedClosure
                var reflection_file = zip.GetEntry($"{name}.{stage}.re")!;

                var blob = GC.AllocateUninitializedArray<byte>((int)obj_file.Length);
                var reflection_blob = GC.AllocateUninitializedArray<byte>((int)reflection_file.Length);

                await using var obj_stream = obj_file.Open();
                await obj_stream.ReadExactlyAsync(blob);

                await using var reflection_stream = reflection_file.Open();
                await reflection_stream.ReadExactlyAsync(reflection_blob);

                return new StageData(shader_stage, blob, reflection_blob);
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
        return new(meta.Id, meta.Path, passes);
    }

    #endregion
}
