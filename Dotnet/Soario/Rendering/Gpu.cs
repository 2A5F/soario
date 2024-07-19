using Soario.Native;

namespace Soario.Rendering;

public sealed class Gpu : IDisposable
{
    #region Static Fields

    internal static Gpu s_gpu = null!;

    public static Gpu Instance => s_gpu;

    #endregion

    #region Fields

    internal unsafe FGpu* m_inner;

    #endregion

    #region Ctor

    internal unsafe Gpu(FGpu* inner)
    {
        m_inner = inner;
    }

    #endregion

    #region Dispose

    private unsafe void ReleaseUnmanagedResources()
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

    ~Gpu() => ReleaseUnmanagedResources();

    #endregion
}
