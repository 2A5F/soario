using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Coplt.Mathematics;
using Soario.Native;

namespace Soario.Windowing;

public class Window : IDisposable
{
    internal unsafe FWindow* inner;
    internal GCHandle self_handle;

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

            inner = FWindow.create(&f_options);

            if (inner == null) throw new WindowException("创建窗口失败");

            self_handle = GCHandle.Alloc(this, GCHandleType.Weak);
            inner->set_gc_handle((void*)GCHandle.ToIntPtr(self_handle));
        }
    }

    #endregion

    #region Dispose

    private unsafe void ReleaseUnmanagedResources()
    {
        if (inner == null) return;
        inner->Release();
        inner = null;
        self_handle.Free();
        self_handle = default;
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

    #region Event

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
            Console.WriteLine(e); //todo logger
        }
    }

    private unsafe void EventHandle(FWindowEventType type, void* data)
    {
        // todo
        Console.WriteLine(type);
    }

    #endregion
}
