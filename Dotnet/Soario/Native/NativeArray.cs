using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Soario.Native;

public sealed unsafe class NativeArray<T> : IDisposable, IList<T>, IEquatable<NativeArray<T>> where T : unmanaged
{
    #region Fields

    internal void* m_ptr;
    internal readonly int m_len;

    #endregion

    #region Prop

    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(m_ptr, Count);
    }

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_len;
    }

    #endregion

    #region Ctor

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NativeArray(int length)
    {
        m_len = length;
        m_ptr = FFI.alloc((nuint)(length * sizeof(T)));
    }

    #endregion

    #region Dispose

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseUnmanagedResources()
    {
        if (m_ptr == null) return;
        FFI.free(m_ptr);
        m_ptr = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ~NativeArray() => ReleaseUnmanagedResources();

    #endregion

    #region Index

    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Span[index];
    }

    #endregion

    #region Enumerator

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T>.Enumerator GetEnumerator() => Span.GetEnumerator();

    public struct Enumerator(NativeArray<T> self) : IEnumerator<T>
    {
        private int m_index = -1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int index = m_index + 1;
            if (index < self.Length)
            {
                m_index = index;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            m_index = -1;
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => self[m_index];
        }

        object IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() { }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator();

    IEnumerator IEnumerable.GetEnumerator() => new Enumerator();

    #endregion

    #region IList

    T IList<T>.this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Span[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Span[index] = value;
    }

    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Contains(T item) => throw new NotSupportedException();
    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Length;
    }
    bool ICollection<T>.IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }
    int IList<T>.IndexOf(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    #endregion

    #region Equals

    public bool Equals(NativeArray<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return m_ptr == other.m_ptr;
    }
    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || obj is NativeArray<T> other && Equals(other);
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => unchecked((int)(long)m_ptr);
    public static bool operator ==(NativeArray<T>? left, NativeArray<T>? right) => Equals(left, right);
    public static bool operator !=(NativeArray<T>? left, NativeArray<T>? right) => !Equals(left, right);

    #endregion

    #region ToString

    public override string ToString() => $"NativeArray({Length})";

    #endregion
}
