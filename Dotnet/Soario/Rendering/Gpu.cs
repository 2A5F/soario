using System.Runtime.CompilerServices;
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

    internal GpuDevice m_main_device = null!;

    #endregion

    #region Getter

    public GpuDevice MainDevice
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_main_device;
    }

    #endregion

    #region Ctor

    internal unsafe Gpu(FGpu* inner)
    {
        m_inner = inner;
        if (m_inner != null)
        {
            m_main_device = new GpuDevice(this, "Main Device");
        }
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

    #region CreateDevice

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuDevice CreateDevice(string? name = null) => new(this, name);

    #endregion
}
