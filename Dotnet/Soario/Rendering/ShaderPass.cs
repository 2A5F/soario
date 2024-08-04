using System.Runtime.CompilerServices;
using Coplt.ShaderReflections;
using Soario.Native;

namespace Soario.Rendering;

public sealed class ShaderPass : IDisposable
{
    #region Fields

    public string Name { get; }

    public ShaderPassStage? Ps { get; }
    public ShaderPassStage? Vs { get; }
    public ShaderPassStage? Cs { get; }
    public ShaderPassStage? Ms { get; }
    public ShaderPassStage? As { get; }

    #endregion

    public ShaderPass(Shader.PassData data)
    {
        Name = data.Name;
        Ps = data.Ps is { } p ? new(p) : null;
        Vs = data.Vs is { } v ? new(v) : null;
        Cs = data.Cs is { } c ? new(c) : null;
        Ms = data.Ms is { } m ? new(m) : null;
        As = data.As is { } a ? new(a) : null;
    }

    #region Dispose

    public void Dispose()
    {
        Ps?.Dispose();
        Vs?.Dispose();
        Cs?.Dispose();
        Ms?.Dispose();
        As?.Dispose();
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
