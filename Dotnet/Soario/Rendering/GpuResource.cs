using System.Runtime.CompilerServices;
using Coplt.Dropping;
using Soario.Native;

namespace Soario.Rendering;

public enum GpuResourceUsage
{
    GpuOnly,
    CpuToGpu,
    GpuToCpu,
}

[Flags]
public enum GpuResourceFlags
{
    None = 0,
    Rtv = 1 << 0,
    Dsv = 1 << 1,
    Uav = 1 << 2,
    Srv = 1 << 3,
    CrossGpu = 1 << 4,
    SharedAccess = 1 << 5,
    RayTracingAccelerationStructure = 1 << 6,

    RtvDsv = Rtv | Dsv,
    Default = Uav | Srv,
}

public record struct GpuBufferCreateOptions()
{
    public string? Name { get; set; }
    public GpuResourceUsage Usage { get; set; } = GpuResourceUsage.GpuOnly;
    public long Size { get; set; }
    public GpuResourceFlags Flags { get; set; } = GpuResourceFlags.Default;

    public GpuBufferCreateOptions(long size) : this()
    {
        Size = size;
    }
}

internal static class GpuResourceUsageEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FGpuResourceUsage ToFFI(this GpuResourceUsage usage) => usage switch
    {
        GpuResourceUsage.GpuOnly => FGpuResourceUsage.GpuOnly,
        GpuResourceUsage.CpuToGpu => FGpuResourceUsage.CpuToGpu,
        GpuResourceUsage.GpuToCpu => FGpuResourceUsage.GpuToCpu,
        _ => throw new ArgumentOutOfRangeException(nameof(usage), usage, null)
    };
}

[Dropping(Unmanaged = true)]
public abstract unsafe partial class GpuResource : IGpuRes
{
    #region Fields

    internal FGpuResource* m_inner;
    internal GpuDevice m_device;
    internal string m_name;

    #endregion

    #region Props

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

    #region Ctor

    protected internal GpuResource(GpuDevice device, string name)
    {
        m_device = device;
        m_name = name;
    }

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

    #region ToString

    public override string ToString() => $"GpuResource({m_name})";

    #endregion

    #region Res

    public abstract ResourceState State { get; }
    public abstract ResourceState ReqState(ResourceState new_state);
    public abstract FGpuRes* AsResPointer();

    #endregion
}

public sealed unsafe class GpuBuffer : GpuResource
{
    #region Ctor

    internal GpuBuffer(GpuDevice device, in GpuBufferCreateOptions options) : base(device,
        options.Name ?? $"Anonymous Buffer ({Guid.NewGuid():D})")
    {
        fixed (char* p_name = m_name)
        {
            var f_options = new FGpuResourceBufferCreateOptions
            {
                name = new FrStr16 { ptr = (ushort*)p_name, len = (nuint)m_name.Length },
                usage = options.Usage.ToFFI(),
                size = options.Size,
                flags = Unsafe.BitCast<GpuResourceFlags, FGpuResourceFlags>(options.Flags),
            };
            FError err;
            m_inner = m_device.m_inner->CreateBuffer(&f_options, &err);
            if (m_inner == null) err.Throw();
        }
    }

    #endregion

    #region ToString

    public override string ToString() => $"GpuBuffer({m_name})";

    #endregion

    #region Res

    public override ResourceState State { get; }
    public override ResourceState ReqState(ResourceState new_state)
    {
        throw new NotImplementedException();
    }
    public override FGpuRes* AsResPointer()
    {
        throw new NotImplementedException();
    }

    #endregion
}
