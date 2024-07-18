using System.Runtime.CompilerServices;
using System.Text;

namespace Soario.Native;

public unsafe partial struct FmStr16
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => new(ptr, (int)len);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() =>
        string.Create((int)len, this, static (span, self) => self.AsSpan().CopyTo(span));
}

public unsafe partial struct FmStr8
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> AsSpan() => new(ptr, (int)len);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Encoding.UTF8.GetString(AsSpan());
}

public unsafe partial struct FrStr16
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<char> AsSpan() => new(ptr, (int)len);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() =>
        string.Create((int)len, this, static (span, self) => self.AsSpan().CopyTo(span));
}

public unsafe partial struct FrStr8
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsSpan() => new(ptr, (int)len);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => Encoding.UTF8.GetString(AsSpan());
}
