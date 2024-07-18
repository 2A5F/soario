using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Utils;

public static unsafe class Time
{
    internal static TimeData* p_time_data;

    /// <summary>
    /// 开始执行时的单调时间
    /// </summary>
    public static TimeSpan StartTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(p_time_data->start_time / 100);
    }
    
    /// <summary>
    /// 上帧的单调时间
    /// </summary>
    public static TimeSpan LastTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(p_time_data->last_time / 100);
    }
    
    /// <summary>
    /// 当前帧的单调时间
    /// </summary>
    public static TimeSpan NowTime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(p_time_data->now_time / 100);
    }

    /// <summary>
    /// 时间增量
    /// </summary>
    public static double Delta
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => p_time_data->delta_time;
    }

    /// <summary>
    /// 距离开始执行过去了多久
    /// </summary>
    public static double Total
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => p_time_data->total_time;
    }

    /// <summary>
    /// 时间增量
    /// </summary>
    public static TimeSpan DeltaRaw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(p_time_data->delta_time_raw / 100);
    }

    /// <summary>
    /// 距离开始执行过去了多久
    /// </summary>
    public static TimeSpan TotalRaw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(p_time_data->total_time_raw / 100);
    }
}
