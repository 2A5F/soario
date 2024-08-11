using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soario.Native;

namespace Soario.Rendering;

public sealed unsafe class GpuTask
{
    #region Fields

    private readonly GpuQueue m_queue;
    internal FGpuTask* m_inner;
    private string m_name;

    private bool m_end;

    #endregion

    #region Props

    public GpuQueue Queue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_queue;
    }

    public string Name
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_name;
    }

    #endregion

    #region Ctor

    internal GpuTask(GpuQueue queue, string? name)
    {
        m_queue = queue;
        m_name = name ?? $"Anonymous Task ({Guid.NewGuid():D})";
        fixed (char* p_name = m_name)
        {
            var options = new FGpuTaskCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
            };
            FError err;
            m_inner = m_queue.m_inner->CreateTask(&options, &err);
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
    ~GpuTask() => Dispose();

    #endregion

    #region ToString

    public override string ToString() => $"GpuTask({m_name})";

    #endregion

    #region Submit

    /// <summary>
    /// 提交命令
    /// </summary>
    public void Submit(GpuCmdList cmd)
    {
        if (m_end) throw new ArgumentException("Cannot submit on a completed task");
        cmd.Submit(this, static (self, list) =>
        {
            FError err;
            self.m_inner->submit(list, &err);
            if (err.type != FErrorType.None) err.Throw();
        });
    }

    #endregion

    #region End

    /// <summary>
    /// 完成任务
    /// </summary>
    public void Complete()
    {
        FError err;
        m_inner->end(&err);
        if (err.type != FErrorType.None) err.Throw();
        m_end = true;
    }

    #endregion

    #region WaitReset

    /// <summary>
    /// 等待任务完成
    /// </summary>
    public void WaitReset()
    {
        FError err;
        m_inner->wait_reset(&err);
        if (err.type != FErrorType.None) err.Throw();
        m_end = false;
    }

    #endregion
}
