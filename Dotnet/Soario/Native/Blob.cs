using System.Runtime.CompilerServices;

namespace Soario.Native;

public readonly struct Blob : IDisposable
{
    private readonly FmStr8 data;

    public Blob(byte[] data)
    {
        this.data = FFI.alloc_str((nuint)data.Length);
        data.CopyTo(this.data.AsSpan());
    }

    public void Dispose()
    {
        FFI.free_str(data);
    }

    public unsafe bool IsNull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => data.ptr == null;
    }

    public Span<byte> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
        get => data.AsSpan();
    }

    public unsafe FrStr8 UnsafeSlice
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new() { ptr = data.ptr, len = data.len };
    }
}
