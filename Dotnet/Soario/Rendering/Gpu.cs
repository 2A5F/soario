using System.Runtime.CompilerServices;
using Coplt.Dropping;
using Soario.Native;

namespace Soario.Rendering;

[Dropping(Unmanaged = true)]
public sealed partial class Gpu
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

    internal static void Init()
    {
        s_gpu = new Gpu();
    }

    internal unsafe Gpu()
    {
        FError err;
        m_inner = FGpu.CreateGpu(&err);
        if (m_inner == null || err.type != FErrorType.None) err.Throw();
        m_main_device = new GpuDevice(this, "Main Device");
    }

    internal unsafe Gpu(FGpu* inner)
    {
        m_inner = inner;
        if (m_inner != null)
        {
            m_main_device = new GpuDevice(this, "Main Device");
        }
    }

    #endregion

    #region Drop

    [Drop]
    private unsafe void Drop()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }

    #endregion

    #region CreateDevice

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuDevice CreateDevice(string? name = null) => new(this, name);

    #endregion
}
