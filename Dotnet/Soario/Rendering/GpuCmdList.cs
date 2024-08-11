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
    internal static FGpuCmdClearRtFlag ToFFI(this ClearFlag self) =>
        Unsafe.BitCast<ClearFlag, FGpuCmdClearRtFlag>(self);
}

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct RasterizerViewPort(float4 rect, float2 depth_range)
{
    /// <summary>
    /// 左上角原点 x y w h
    /// </summary>
    [FieldOffset(0)]
    public float4 Rect = rect;
    /// <summary>
    /// min max
    /// </summary>
    [FieldOffset(16)]
    public float2 DepthRange = depth_range;
}

[StructLayout(LayoutKind.Explicit, Size = 40)]
public struct RasterizerInfo(float4 rect, float2 depth_range, int4 scissor_rect)
{
    [FieldOffset(0)]
    public int4 ScissorRect = scissor_rect;
    [FieldOffset(16)]
    public RasterizerViewPort ViewPort = new(rect, depth_range);
}

public sealed unsafe class GpuCmdList
{
    #region Fields

    internal readonly GpuDevice m_device;
    internal readonly List<int> m_indexes = new();
    internal readonly List<byte> m_datas = new();
    /// <summary>
    /// 保留引用，避免释放
    /// </summary>
    // ReSharper disable once CollectionNeverQueried.Global
    internal readonly HashSet<object> m_objects = new();

    internal IRt? m_current_rtv;
    internal IRt? m_current_dsv;

    internal bool m_is_bind_less = true;

    public GpuCmdList(GpuDevice device)
    {
        m_device = device;
        if (m_is_bind_less) StartBindLess();
    }

    #endregion

    #region Reset

    internal void Reset()
    {
        m_indexes.Clear();
        m_datas.Clear();
        m_objects.Clear();
        m_current_rtv = null;
        m_current_dsv = null;
        if (m_is_bind_less) StartBindLess();
    }

    #endregion

    #region Cmds

    #region SetRt

    /// <summary>
    /// 设置渲染目标
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRt(IRt rt)
    {
        var has_rtv = rt.HasRtv;
        var has_dsv = rt.HasDsv;
        if (has_rtv) m_current_rtv = rt;
        if (has_dsv) m_current_dsv = rt;
        if (!has_rtv && !has_dsv) throw new ArgumentException("Invalid rt", nameof(rt));
        else
        {
            var old_state = rt.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(rt, old_state, ResourceState.RenderTarget);
            else m_objects.Add(rt); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
    }

    /// <summary>
    /// 设置渲染目标
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetRt(IRt rtv, IRt dsv)
    {
        if (!rtv.HasRtv) throw new ArgumentException("Invalid rtv", nameof(rtv));
        {
            m_current_rtv = rtv;
            var old_state = rtv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(rtv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(rtv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
        if (!dsv.HasDsv) throw new ArgumentException("Invalid dsv", nameof(dsv));
        {
            m_current_dsv = dsv;
            var old_state = dsv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(dsv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(dsv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
    }

    #endregion

    #region Clear

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv) => Clear(rtv, dsv, default, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt)
    {
        var has_rtv = rt.HasRtv;
        var has_dsv = rt.HasDsv;
        if (has_rtv && has_dsv) Clear(rt, default, 0, 0);
        else if (has_rtv) Clear(rt, default(float4));
        else if (has_dsv) Clear(rt, 0, 0);
        else throw new ArgumentException("Invalid rt", nameof(rt));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, float4 color) =>
        Clear(rtv, color, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt dsv, float depth) =>
        Clear(dsv, ClearFlag.Depth, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt dsv, byte stencil) =>
        Clear(dsv, ClearFlag.Stencil, 0, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, float depth) =>
        Clear(rt, rt, ClearFlag.ColorDepth, color, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv, float4 color, float depth) =>
        Clear(rtv, dsv, ClearFlag.ColorDepth, color, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, byte stencil) =>
        Clear(rt, rt, ClearFlag.ColorStencil, color, 0, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv, float4 color, byte stencil) =>
        Clear(rtv, dsv, ClearFlag.ColorStencil, color, 0, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt dsv, float depth, byte stencil) =>
        Clear(dsv, ClearFlag.DepthStencil, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, float4 color, float depth, byte stencil) =>
        Clear(rt, rt, ClearFlag.All, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv, float4 color, float depth, byte stencil) =>
        Clear(rtv, dsv, ClearFlag.All, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rt, ClearFlag flag, float4 color, float depth, byte stencil) =>
        Clear(rt, rt, flag, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv, ClearFlag flag, float4 color, float depth, byte stencil) =>
        Clear(rtv, dsv, flag, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, float4 color, ReadOnlySpan<int4> rects)
    {
        if (!rtv.HasRtv) throw new ArgumentException("Invalid rtv", nameof(rtv));
        {
            var old_state = rtv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(rtv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(rtv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdClearRt
        {
            type = FGpuCmdType.ClearRt,
            flag = ClearFlag.Color.ToFFI(),
            rtv = rtv.AsRtPointer(),
            dsv = null,
            color = Unsafe.BitCast<float4, FFloat4>(color),
            depth = 0,
            stencil = 0,
            rect_len = rects.Length,
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
        m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt dsv, ClearFlag flag, float depth, byte stencil, ReadOnlySpan<int4> rects)
    {
        if (!dsv.HasDsv) throw new ArgumentException("Invalid dsv", nameof(dsv));
        {
            var old_state = dsv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(dsv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(dsv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdClearRt
        {
            type = FGpuCmdType.ClearRt,
            flag = flag.ToFFI(),
            rtv = null,
            dsv = dsv.AsRtPointer(),
            color = default,
            depth = depth,
            stencil = stencil,
            rect_len = rects.Length,
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
        m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(IRt rtv, IRt dsv, ClearFlag flag, float4 color, float depth, byte stencil,
        ReadOnlySpan<int4> rects)
    {
        if (!rtv.HasRtv) throw new ArgumentException("Invalid rtv", nameof(rtv));
        {
            var old_state = rtv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(rtv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(rtv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
        if (!dsv.HasDsv) throw new ArgumentException("Invalid dsv", nameof(dsv));
        {
            var old_state = dsv.ReqState(ResourceState.RenderTarget);
            if (old_state != ResourceState.RenderTarget) BarrierTransition(dsv, old_state, ResourceState.RenderTarget);
            else m_objects.Add(dsv); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
        }
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdClearRt
        {
            type = FGpuCmdType.ClearRt,
            flag = flag.ToFFI(),
            rtv = rtv.AsRtPointer(),
            dsv = dsv.AsRtPointer(),
            color = Unsafe.BitCast<float4, FFloat4>(color),
            depth = depth,
            stencil = stencil,
            rect_len = rects.Length,
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
        m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() =>
        Clear(ClearFlag.All, default, 0, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float4 color) =>
        Clear(color, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float depth) =>
        Clear(ClearFlag.Depth, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(byte stencil) =>
        Clear(ClearFlag.DepthStencil, 0, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float depth, byte stencil) =>
        Clear(ClearFlag.DepthStencil, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float4 color, float depth) =>
        Clear(ClearFlag.ColorDepth, color, depth, 0, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float4 color, byte stencil) =>
        Clear(ClearFlag.ColorStencil, color, 0, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(ClearFlag flag, float4 color, float depth, byte stencil) =>
        Clear(flag, color, depth, stencil, []);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(float4 color, ReadOnlySpan<int4> rects)
    {
        if (m_current_rtv is not null)
        {
            m_indexes.Add(m_datas.Count);
            var data = new FGpuCmdClearRt
            {
                type = FGpuCmdType.ClearRt,
                flag = ClearFlag.Color.ToFFI(),
                rtv = m_current_rtv.AsRtPointer(),
                dsv = null,
                color = Unsafe.BitCast<float4, FFloat4>(color),
                depth = 0,
                stencil = 0,
                rect_len = rects.Length,
            };
            m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
            m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
        }
        else throw new ArgumentException("Rt not set");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(ClearFlag flag, float depth, byte stencil, ReadOnlySpan<int4> rects)
    {
        if (m_current_dsv is not null)
        {
            m_indexes.Add(m_datas.Count);
            var data = new FGpuCmdClearRt
            {
                type = FGpuCmdType.ClearRt,
                flag = flag.ToFFI(),
                rtv = null,
                dsv = m_current_dsv.AsRtPointer(),
                color = default,
                depth = depth,
                stencil = stencil,
                rect_len = rects.Length,
            };
            m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
            m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
        }
        else throw new ArgumentException("Rt not set");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(ClearFlag flag, float4 color, float depth, byte stencil,
        ReadOnlySpan<int4> rects)
    {
        if (m_current_rtv is not null && m_current_dsv is not null)
        {
            m_indexes.Add(m_datas.Count);
            var data = new FGpuCmdClearRt
            {
                type = FGpuCmdType.ClearRt,
                flag = flag.ToFFI(),
                rtv = m_current_rtv.AsRtPointer(),
                dsv = m_current_dsv.AsRtPointer(),
                color = Unsafe.BitCast<float4, FFloat4>(color),
                depth = depth,
                stencil = stencil,
                rect_len = rects.Length,
            };
            m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
            m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
        }
        else if (m_current_rtv is not null)
        {
            m_indexes.Add(m_datas.Count);
            var data = new FGpuCmdClearRt
            {
                type = FGpuCmdType.ClearRt,
                flag = flag.ToFFI(),
                rtv = m_current_rtv.AsRtPointer(),
                dsv = null,
                color = Unsafe.BitCast<float4, FFloat4>(color),
                depth = depth,
                stencil = stencil,
                rect_len = rects.Length,
            };
            m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
            m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
        }
        else if (m_current_dsv is not null)
        {
            m_indexes.Add(m_datas.Count);
            var data = new FGpuCmdClearRt
            {
                type = FGpuCmdType.ClearRt,
                flag = flag.ToFFI(),
                rtv = null,
                dsv = m_current_dsv.AsRtPointer(),
                color = Unsafe.BitCast<float4, FFloat4>(color),
                depth = depth,
                stencil = stencil,
                rect_len = rects.Length,
            };
            m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdClearRt)));
            m_datas.AddRange(MemoryMarshal.Cast<int4, byte>(rects));
        }
        else throw new ArgumentException("Rt not set");
    }

    #endregion

    #region Barrier

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BarrierTransition(IGpuRes res, ResourceState old_state, ResourceState cur_state) =>
        BarrierTransition(res, uint.MaxValue, old_state, cur_state);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BarrierTransition(IGpuRes res, uint sub_res, ResourceState old_state, ResourceState cur_state)
    {
        m_objects.Add(res);
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdBarrierTransition
        {
            type = FGpuCmdType.BarrierTransition,
            sub_res = sub_res,
            res = res.AsResPointer(),
            pre_state = old_state.ToFFI(),
            cur_state = cur_state.ToFFI(),
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdBarrierTransition)));
    }

    #endregion

    #region Present

    public void Present(IRt rt)
    {
        var old_state = rt.ReqState(ResourceState.Common);
        if (old_state != ResourceState.Common) BarrierTransition(rt, old_state, ResourceState.Common);
        else m_objects.Add(rt); // BarrierTransition 会缓存对象引用，所以只有不调用的时候需要 Add
    }

    #endregion

    #region ReadyRasterizer

    /// <summary>
    /// 准备光栅
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadyRasterizer(RasterizerInfo rasterizer_infos) => ReadyRasterizer([rasterizer_infos]);

    /// <summary>
    /// 准备光栅
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadyRasterizer(ReadOnlySpan<RasterizerInfo> rasterizer_infos)
    {
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdReadyRasterizer
        {
            type = FGpuCmdType.ReadyRasterizer,
            len = rasterizer_infos.Length,
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdReadyRasterizer)));
        m_datas.AddRange(MemoryMarshal.Cast<RasterizerInfo, byte>(rasterizer_infos));
    }

    #endregion

    #region BindLess

    public void StartBindLess()
    {
        m_is_bind_less = true;
        // todo set layout
    }

    public void StopBindLess()
    {
        throw new NotSupportedException();
    }

    #endregion

    #region DispatchMesh

    /// <summary>
    /// 派发 Mesh Shader
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DispatchMesh(ShaderPass pass, uint3 thread_groups)
    {
        if (m_current_rtv is null) throw new ArgumentException("Rt not set");
        var format = m_current_rtv.Format;
        var formats = new GpuTextureFormats(format);
        // todo depth
        var pipeline = pass.GetOrCreatePipelineState(m_device, formats, GpuDepthFormat.Unknown);

        DispatchMesh(pipeline, thread_groups);
    }

    /// <summary>
    /// 派发 Mesh Shader
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DispatchMesh(GpuPipelineState pipeline, uint3 thread_groups)
    {
        m_objects.Add(pipeline);
        m_indexes.Add(m_datas.Count);
        var data = new FGpuCmdDispatchMesh
        {
            type = FGpuCmdType.DispatchMesh,
            pipeline = pipeline.m_inner,
            thread_groups = Unsafe.BitCast<uint3, FUInt3>(thread_groups),
        };
        m_datas.AddRange(new Span<byte>(&data, sizeof(FGpuCmdDispatchMesh)));
    }

    #endregion

    #endregion
}
