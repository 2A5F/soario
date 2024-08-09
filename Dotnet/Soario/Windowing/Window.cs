using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Coplt.Mathematics;
using Serilog;
using Soario.Native;

namespace Soario.Windowing;

public class Window : IDisposable
{
    internal unsafe FWindow* m_inner;
    internal GCHandle m_self_handle;

    #region Create

    public record struct Options
    {
        public required string Title { get; set; }
        public required int2 Size { get; set; }
        public int2? MinSize { get; set; }
    }

    public unsafe Window(Options options)
    {
        fixed (char* p_title = options.Title)
        {
            var f_options = new WindowCreateOptions
            {
                title = new() { ptr = (ushort*)p_title, len = (nuint)options.Title.Length },
                size = Unsafe.BitCast<int2, FInt2>(options.Size),
                min_size = options.MinSize.HasValue ? Unsafe.BitCast<int2, FInt2>(options.MinSize.Value) : default,
                has_min_size = options.MinSize.HasValue ? 1 : 0,
            };

            FError err;
            m_inner = FWindow.create(&err, &f_options);

            if (err) err.Throw();

            if (m_inner == null) throw new WindowException("创建窗口失败");

            m_self_handle = GCHandle.Alloc(this, GCHandleType.Weak);
            m_inner->set_gc_handle((void*)GCHandle.ToIntPtr(m_self_handle));
        }
    }

    #endregion

    #region Dispose

    private unsafe void ReleaseUnmanagedResources()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
        m_self_handle.Free();
        m_self_handle = default;
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

    ~Window() => Dispose(false);

    #endregion

    #region GetSize

    /// <summary>
    /// 获取窗口大小
    /// </summary>
    public unsafe int2 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            FError err;
            FInt2 f_size;
            m_inner->get_size(&err, &f_size);
            if (err) err.Throw();
            return Unsafe.BitCast<FInt2, int2>(f_size);
        }
    }

    #endregion

    #region Event

    public event Action? OnClose; 
    public event Action? OnResize; 

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe void EventHandle(void* gc_handle, FWindowEventType type, void* data)
    {
        try
        {
            var handle = GCHandle.FromIntPtr((IntPtr)gc_handle);
            if (handle.Target is not Window win) return;
            win.EventHandle(type, data);
        }
        catch (Exception e)
        {
            Log.Error(e, "");
        }
    }

    private unsafe void EventHandle(FWindowEventType type, void* data)
    {
        switch (type)
        {
            case FWindowEventType.Close:
                OnClose?.Invoke();;
                break;
            case FWindowEventType.Resize:
                OnResize?.Invoke();
                break;
        }
    }

    #endregion
}
