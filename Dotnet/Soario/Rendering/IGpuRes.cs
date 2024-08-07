using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public interface IGpuRes
{
    internal ResourceState State
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    
    /// <summary>
    /// 请求状态
    /// </summary>
    /// <returns>返回老状态</returns>
    internal ResourceState ReqState(ResourceState new_state);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe FGpuRes* AsResPointer();
}
