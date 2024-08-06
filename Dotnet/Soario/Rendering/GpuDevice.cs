using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;
using Serilog.Events;
using Soario.Native;

namespace Soario.Rendering;

public sealed unsafe class GpuDevice : IDisposable
{
    #region Fields

    private readonly Gpu m_gpu;
    internal FGpuDevice* m_inner;
    private string m_name;

    private GpuQueue m_queue_common;
    private GpuQueue m_queue_compute;
    private GpuQueue m_queue_copy;

    #endregion

    #region Props

    public Gpu Gpu
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_gpu;
    }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_name;
    }
    
    public GpuQueue CommonQueue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_queue_common;
    }
    
    public GpuQueue ComputeQueue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_queue_compute;
    }
    
    public GpuQueue CopyQueue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_queue_copy;
    }

    #endregion

    #region Ctor

    internal GpuDevice(Gpu gpu, string? name)
    {
        m_name = name ??= $"Anonymous Device ({Guid.NewGuid():D})";
        m_gpu = gpu;
        fixed (char* p_name = name)
        {
            var options = new FGpuDeviceCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)name.Length },
                logger = &DebugLogger,
                logger_object = null,
                logger_drop_object = null,
            };
            FError err;
            m_inner = m_gpu.m_inner->CreateDevice(&options, &err);
            if (m_inner == null) err.Throw();
        }
        m_queue_common = new GpuQueue(this, GpuQueue.Kind.Common, $"{m_name} Common Queue");
        m_queue_compute = new GpuQueue(this, GpuQueue.Kind.Common, $"{m_name} Compute Queue");
        m_queue_copy = new GpuQueue(this, GpuQueue.Kind.Common, $"{m_name} Copy Queue");
    }

    #endregion

    #region DebugLogger

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void DebugLogger(void* obj, FLogLevel level, sbyte* msg)
    {
        var str = Marshal.PtrToStringUTF8((IntPtr)msg);
        Log.Write(level.ToLogEventLevel(), "[DirectX] {Message}", str);
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
    ~GpuDevice() => Dispose();

    #endregion

    #region ToString

    public override string ToString() => $"GpuDevice({m_name})";

    #endregion
}
