using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Coplt.Mathematics;
using Soario.Native;

namespace Soario.Rendering;

public enum ClearFlag : byte
{
    None = 0,
    Color = 1 << 0,
    Depth = 1 << 2,
    Stencil = 1 << 3,

    DepthStencil = Depth | Stencil,
    ColorDepth = Color | Depth,
    ColorStencil = Color | Stencil,
    All = Color | Depth | Stencil,
}

public static class ClearFlagEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static FGpuCmdClearRtvFlag ToFFI(this ClearFlag self) =>
        Unsafe.BitCast<ClearFlag, FGpuCmdClearRtvFlag>(self);
}

public sealed unsafe class GpuCmdList
{
    internal List<int> indexes = new();
    internal List<byte> datas = new();
    /// <summary>
    /// 保留引用，避免释放
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    internal HashSet<object> objects = new();

    #region Reset

    internal void Reset()
    {
        indexes.Clear();
        datas.Clear();
        objects.Clear();
    }

    #endregion

    #region Cmds

    /// <summary>
    /// 用 0 清空 Rt
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt) => Clear(rt, default, -1, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color) =>
        Clear(rt, ClearFlag.Color, color, -1, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float depth) =>
        Clear(rt, ClearFlag.Depth, default, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, byte stencil) =>
        Clear(rt, ClearFlag.Stencil, default, -1, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, float depth) =>
        Clear(rt, ClearFlag.ColorDepth, color, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, byte stencil) =>
        Clear(rt, ClearFlag.ColorStencil, color, -1, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float depth, byte stencil) =>
        Clear(rt, ClearFlag.DepthStencil, default, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, float depth, byte stencil) =>
        Clear(rt, ClearFlag.All, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, ClearFlag flag, float4 color, float depth, byte stencil) =>
        Clear(rt, flag, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, ClearFlag flag, float4 color, float depth, byte stencil, ReadOnlySpan<int4> rects)
    {
        var old_state = rt.ReqState(ResourceState.RenderTarget);
        if (old_state != ResourceState.RenderTarget) BarrierTransition(rt, old_state, ResourceState.RenderTarget);
        else objects.Add(rt); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        indexes.Add(datas.Count);
        var data = new FGpuCmdClearRtv
        {
            type = FGpuCmdType.ClearRtv,
            flag = flag.ToFFI(),
            rt = rt.AsRtPointer(),
            color = Unsafe.BitCast<float4, FFloat4>(color),
            depth = depth,
            stencil = stencil,
            rect_len = rects.Length,
        };
        datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRtv)));
        datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
    }

    #endregion

    #region Barrier

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BarrierTransition(IGpuRes res, ResourceState old_state, ResourceState cur_state) =>
        BarrierTransition(res, uint.MaxValue, old_state, cur_state);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BarrierTransition(IGpuRes res, uint sub_res, ResourceState old_state, ResourceState cur_state)
    {
        objects.Add(res);
        indexes.Add(datas.Count);
        var data = new FGpuCmdBarrierTransition
        {
            type = FGpuCmdType.BarrierTransition,
            sub_res = sub_res,
            res = res.AsResPointer(),
            pre_state = old_state.ToFFI(),
            cur_state = cur_state.ToFFI(),
        };
        datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdBarrierTransition)));
    }

    #endregion

    #region Present

    public void Present(IRt rt)
    {
        var old_state = rt.ReqState(ResourceState.Common);
        if (old_state != ResourceState.Common) BarrierTransition(rt, old_state, ResourceState.Common);
        else objects.Add(rt); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
    }

    #endregion
}
