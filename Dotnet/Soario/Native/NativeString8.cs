using System.Runtime.CompilerServices;
using System.Text;
using Coplt.Dropping;

namespace Soario.Native;

[Dropping(Unmanaged = true)]
public sealed unsafe partial class NativeString8
{
    #region Fields

    internal FString8* m_inner;

    #endregion

    #region Ctor

    public NativeString8(ReadOnlySpan<byte> bytes)
    {
        fixed (byte* ptr = bytes)
        {
            m_inner = FString8.Create(new() { ptr = ptr, len = (nuint)bytes.Length });
        }
    }

    public NativeString8(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        fixed (byte* ptr = bytes)
        {
            m_inner = FString8.Create(new() { ptr = ptr, len = (nuint)bytes.Length });
        }
    }

    #endregion

    #region Props

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_inner == null ? 0 : (int)m_inner->m_len;
    }

    public ReadOnlySpan<byte> AsSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_inner == null ? default : new(m_inner->m_ptr, (int)m_inner->m_len);
    }

    #endregion

    #region ToString

    public override string ToString() => Encoding.UTF8.GetString(AsSpan);

    #endregion

    #region Drop

    [Drop]
    private void Drop()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }

    #endregion
}
