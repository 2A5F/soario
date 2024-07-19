using System.Collections.Frozen;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using Soario.Resources;

namespace Soario.Shading;

public sealed class Shader : AAsset, IEquatable<Shader>
{
    #region Fields

    private FrozenDictionary<string, Pass> Passes { get; }

    #endregion

    #region Ctor

    internal Shader(Guid id, string? path, FrozenDictionary<string, Pass> passes) : base(id, path)
    {
        Passes = passes;
        foreach (var (_, pass) in passes)
        {
            pass.Shader = this;
        }
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

    public sealed class Pass
    {
        #region Fields

        public Shader Shader { get; internal set; } = null!;

        private readonly byte[] m_blob;
        private readonly byte[] m_reflection_blob;

        public ReadOnlyMemory<byte> Blob => m_blob;
        public ReadOnlyMemory<byte> ReflectionBlob => m_reflection_blob;

        #endregion

        #region Ctor

        internal Pass(byte[] blob, byte[] reflection_blob)
        {
            m_blob = blob;
            m_reflection_blob = reflection_blob;
        }

        #endregion
    }

    #endregion

    #region Load

    private static readonly JsonSerializerOptions s_json_serializer_options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static async ValueTask<Shader> Load(string path)
    {
        using var zip = ZipFile.OpenRead(path);
        var meta_file = zip.GetEntry("meta.json")!;
        await using var meta_stream = meta_file.Open();
        var meta = await JsonSerializer.DeserializeAsync<ShaderMeta>(meta_stream, s_json_serializer_options);
        var dict = new Dictionary<string, Pass>();
        foreach (var (name, item) in meta.Items)
        {
            var obj_file = zip.GetEntry($"{name}.o")!;
            await using var obj_stream = obj_file.Open();
            var blob = GC.AllocateUninitializedArray<byte>((int)obj_file.Length);
            await obj_stream.ReadExactlyAsync(blob);

            var reflection_file = zip.GetEntry($"{name}.re")!;
            await using var reflection_stream = reflection_file.Open();
            var reflection_blob = GC.AllocateUninitializedArray<byte>((int)reflection_file.Length);
            await reflection_stream.ReadExactlyAsync(reflection_blob);
            
            // todo native load

            dict[name] = new Pass(blob, reflection_blob);
        }
        return new(meta.Id, meta.Path, dict.ToFrozenDictionary());
    }

    #endregion
}
