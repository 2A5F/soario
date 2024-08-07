using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public abstract unsafe class GpuPipelineLayout : IDisposable
{
    #region Fields

    internal GpuDevice m_device;
    internal FGpuPipelineLayout* m_inner;
    internal string m_name;

    #endregion

    #region Props

    public GpuDevice Device
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_device;
    }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_name;
    }

    #endregion

    #region Ctor

    protected GpuPipelineLayout(GpuDevice device, string name)
    {
        m_device = device;
        m_name = name;
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }
    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~GpuPipelineLayout() => Dispose(false);

    #endregion

    #region ToString

    public override string ToString() => $"GpuPipelineLayout({m_name})";

    #endregion
}

public sealed unsafe class BindLessGpuPipelineLayout : GpuPipelineLayout
{
    #region Ctor

    internal BindLessGpuPipelineLayout(GpuDevice device, string? name) : base(device,
        name ?? $"Anonymous BindLess Pipeline Layout ({Guid.NewGuid():D})")
    {
        fixed (char* p_name = m_name)
        {
            var options = new FGpuBindLessPipelineLayoutCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
            };
            FError err;
            m_inner = device.m_inner->CreateBindLessPipelineLayout(&options, &err);
            if (m_inner == null) err.Throw();
        }
    }

    #endregion
}
