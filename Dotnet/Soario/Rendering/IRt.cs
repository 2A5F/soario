﻿using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public interface IRt : IGpuRes
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe FGpuRt* AsRtPointer();

    public bool HasRtv
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    public bool HasDsv
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
}
