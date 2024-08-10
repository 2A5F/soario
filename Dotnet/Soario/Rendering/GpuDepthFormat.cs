using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public enum GpuDepthFormat
{
    Unknown = 0,
    D16_UNorm = 55,
    D32_Float = 40,
    D24_UNorm_S8_UInt = 45,
    D32_Float_S8_UInt = 20,
}

public static partial class GpuFormatEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static FGpuTextureFormat ToFFI(this GpuDepthFormat format) => format switch
    {
        GpuDepthFormat.Unknown => FGpuTextureFormat.Unknown,
        GpuDepthFormat.D16_UNorm => FGpuTextureFormat.D16_UNorm,
        GpuDepthFormat.D32_Float => FGpuTextureFormat.D32_Float,
        GpuDepthFormat.D24_UNorm_S8_UInt => FGpuTextureFormat.D24_UNorm_S8_UInt,
        GpuDepthFormat.D32_Float_S8_UInt => FGpuTextureFormat.D32_Float_S8X24_UInt,
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
    };
}
