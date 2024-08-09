using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Soario.Native;

namespace Soario.Utilities;

public sealed unsafe class NativeUtf8StringList : IDisposable
{
    #region Fields

    private byte* m_bytes;
    private int m_len;
    private int m_cap;
    private readonly List<FrStr8> m_strs = new();

    #endregion

    #region Getter

    public ReadOnlySpan<FrStr8> Slices
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => CollectionsMarshal.AsSpan(m_strs);
    }

    #endregion

    #region MyRegion

    public NativeUtf8StringList(int cap = 1024)
    {
        if (cap < 4) throw new ArgumentException(nameof(cap));
        m_len = 0;
        m_cap = cap;
        m_bytes = (byte*)FFI.alloc((nuint)cap);
        if (m_bytes == null) throw new OutOfMemoryException();
    }

    #endregion

    #region Grow

    private void Grow(int need)
    {
        var len = (int)BitOperations.RoundUpToPowerOf2((uint)need);
        var ptr = (byte*)FFI.alloc((nuint)len);
        if (ptr == null) throw new OutOfMemoryException();
        var old_span = new Span<byte>(m_bytes, (int)m_len);
        m_bytes = ptr;
        m_cap = len;
    }

    #endregion

    #region Add

    public void Add(ReadOnlySpan<byte> str)
    {
        var new_len = m_len + str.Length + 1;
        if (new_len > m_cap) Grow(new_len);
        var ptr = m_bytes + m_len;
        var span = new Span<byte>(ptr, str.Length + 1);
        str.CopyTo(span);
        span[str.Length] = 0;
        m_len = new_len;
        m_strs.Add(new(ptr, str.Length));
    }

    public void Add(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        Add(bytes);
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (m_bytes != null)
        {
            FFI.free(m_bytes);
            m_bytes = null;
        }
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    
    ~NativeUtf8StringList() => ReleaseUnmanagedResources();

    #endregion
}
