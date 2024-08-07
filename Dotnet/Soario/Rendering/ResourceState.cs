using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public enum ResourceState
{
    Common,
    RenderTarget,
}

public static class ResourceStateEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static FGpuResState ToFFI(this ResourceState state) => state switch
    {
        ResourceState.Common => FGpuResState.Common,
        ResourceState.RenderTarget => FGpuResState.RenderTarget,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}
