using System.Runtime.CompilerServices;
using Coplt.Mathematics;
using Soario.Native;

namespace Soario.Windowing;

public class Window : IDisposable
{
    internal unsafe FWindow* inner;

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
        }
    }

    #endregion

    #region Dispose

    private unsafe void ReleaseUnmanagedResources()
    {
        if (inner == null) return;
        inner->Release();
        inner = null;
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
}
