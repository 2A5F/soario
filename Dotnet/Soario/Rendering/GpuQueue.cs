﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soario.Native;

namespace Soario.Rendering;

public sealed unsafe class GpuQueue : IDisposable
{
    #region Fields

    private readonly Gpu m_gpu;
    private readonly GpuDevice m_device;
    internal FGpuQueue* m_inner;
    private string m_name;

    #endregion

    #region Props

    public Gpu Gpu
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_gpu;
    }

    public GpuDevice Device
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_device;
    }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_name;
    }

    #endregion

    #region Kind

    public enum Kind
    {
        Common,
        Compute,
        Copy,
    }

    #endregion

    #region Ctor

    internal GpuQueue(GpuDevice device, Kind kind, string? name)
    {
        m_device = device;
        m_gpu = device.Gpu;
        m_name = name ??= $"Anonymous Queue ({Guid.NewGuid():D})";
        var type = kind switch
        {
            Kind.Common => FGpuQueueType.Common,
            Kind.Compute => FGpuQueueType.Compute,
            Kind.Copy => FGpuQueueType.Copy,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
        fixed (char* p_name = name)
        {
            var options = new FGpuQueueCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)name.Length },
                type = type,
            };
            FError err;
            m_inner = m_device.m_inner->CreateQueue(&options, &err);
            if (m_inner == null) err.Throw();
        }
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
    ~GpuQueue() => Dispose();

    #endregion

    #region ToString

    public override string ToString() => $"GpuQueue({m_name})";

    #endregion

    #region Submit

    /// <summary>
    /// 提交命令
    /// </summary>
    public void Submit(GpuCmdList cmds)
    {
        fixed (byte* datas = CollectionsMarshal.AsSpan(cmds.datas))
        {
            fixed (int* indexes = CollectionsMarshal.AsSpan(cmds.indexes))
            {
                var list = new FGpuCmdList
                {
                    datas = datas,
                    indexes = indexes,
                    len = (nuint)cmds.indexes.Count,
                };
                FError err;
                m_inner->submit(&list, &err);
                if (err.type != FErrorType.None) err.Throw();
            }
        }
        cmds.Reset();
    }

    #endregion
}
