using System.Runtime.CompilerServices;
using Soario.Native;
using Soario.Windowing;

namespace Soario.Rendering;

public record struct GpuSurfaceCreateOptions()
{
    public string? Name { get; set; }
    public bool VSync { get; set; } = false;
}

public sealed unsafe class GpuSurface : IDisposable, IRt
{
    #region Fields

    private readonly Gpu m_gpu;
    private readonly GpuDevice m_device;
    internal FGpuSurface* m_inner;
    private string m_name;

    #endregion

    #region Props

    public Gpu Gpu
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_gpu;
    }

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

    internal GpuSurface(GpuDevice device, Window window, GpuSurfaceCreateOptions options)
    {
        m_name = options.Name ?? $"Anonymous Surface ({Guid.NewGuid():D})";
        m_gpu = device.Gpu;
        m_device = device;
        fixed (char* p_name = m_name)
        {
            var n_options = new FGpuSurfaceCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
                v_sync = (byte)(options.VSync ? 1 : 0),
            };
            FError err;
            m_inner = m_device.m_inner->CreateSurfaceFromWindow(
                m_device.CommonQueue.m_inner, &n_options, window.m_inner, &err
            );
            if (m_inner == null) err.Throw();
        }
    }

    internal GpuSurface(GpuDevice device, nuint hwnd, GpuSurfaceCreateOptions options)
    {
        m_name = options.Name ?? $"Anonymous Surface ({Guid.NewGuid():D})";
        m_gpu = device.Gpu;
        m_device = device;
        fixed (char* p_name = m_name)
        {
            var n_options = new FGpuSurfaceCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
                v_sync = (byte)(options.VSync ? 1 : 0),
            };
            FError err;
            m_inner = m_device.m_inner->CreateSurfaceFromHwnd(
                m_device.CommonQueue.m_inner, &n_options, hwnd, &err
            );
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
    ~GpuSurface() => Dispose();

    #endregion

    #region ToString

    public override string ToString() => $"GpuSurface({m_name})";

    #endregion

    #region Frame

    public void ReadyFrame(GpuQueue queue)
    {
        FError err;
        m_inner->ready_frame(queue.m_inner, &err);
        if (err.type != FErrorType.None) err.Throw();
    }

    public void PresentFrame()
    {
        FError err;
        m_inner->present_frame(&err);
        if (err.type != FErrorType.None) err.Throw();
    }

    public void PresentFrame(GpuCmdList list, GpuQueue queue)
    {
        list.Present(this);
        queue.Submit(list);
        PresentFrame();
    }

    #endregion

    #region Rt

    public ResourceState State
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal set;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ResourceState ReqState(ResourceState new_state)
    {
        var old = State;
        State = new_state;
        return old;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FGpuRes* AsResPointer()=> (FGpuRes*)m_inner;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FGpuRt* AsRtPointer() => (FGpuRt*)m_inner;

    #endregion
}
