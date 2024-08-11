using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Soario.Native;

namespace Soario.Rendering;

public enum GpuTextureFormat
{
    Unknown = 0,
    R32G32B32A32_TypeLess = 1,
    R32G32B32A32_Float = 2,
    R32G32B32A32_UInt = 3,
    R32G32B32A32_SInt = 4,
    R32G32B32_TypeLess = 5,
    R32G32B32_Float = 6,
    R32G32B32_UInt = 7,
    R32G32B32_SInt = 8,
    R16G16B16A16_TypeLess = 9,
    R16G16B16A16_Float = 10,
    R16G16B16A16_UNorm = 11,
    R16G16B16A16_UInt = 12,
    R16G16B16A16_SNorm = 13,
    R16G16B16A16_SInt = 14,
    R32G32_TypeLess = 15,
    R32G32_Float = 16,
    R32G32_UInt = 17,
    R32G32_SInt = 18,
    R32G8X24_TypeLess = 19,
    D32_Float_S8X24_UInt = 20,
    R32_Float_X8X24_TypeLess = 21,
    X32_TypeLess_G8X24_Float = 22,
    R10G10B10A2_TypeLess = 23,
    R10G10B10A2_UNorm = 24,
    R10G10B10A2_UInt = 25,
    R11G11B10_Float = 26,
    R8G8B8A8_TypeLess = 27,
    R8G8B8A8_UNorm = 28,
    R8G8B8A8_UNorm_sRGB = 29,
    R8G8B8A8_UInt = 30,
    R8G8B8A8_SNorm = 31,
    R8G8B8A8_SInt = 32,
    R16G16_TypeLess = 33,
    R16G16_Float = 34,
    R16G16_UNorm = 35,
    R16G16_UInt = 36,
    R16G16_SNorm = 37,
    R16G16_SInt = 38,
    R32_TypeLess = 39,
    D32_Float = 40,
    R32_Float = 41,
    R32_UInt = 42,
    R32_SInt = 43,
    R24G8_TypeLess = 44,
    D24_UNorm_S8_UInt = 45,
    R24_UNorm_X8_TypeLess = 46,
    X24_TypeLess_G8_UInt = 47,
    R8G8_TypeLess = 48,
    R8G8_UNorm = 49,
    R8G8_UInt = 50,
    R8G8_SNorm = 51,
    R8G8_SInt = 52,
    R16_TypeLess = 53,
    R16_Float = 54,
    D16_UNorm = 55,
    R16_UNorm = 56,
    R16_UInt = 57,
    R16_SNorm = 58,
    R16_SInt = 59,
    R8_TypeLess = 60,
    R8_UNorm = 61,
    R8_UInt = 62,
    R8_SNorm = 63,
    R8_SInt = 64,
    A8_UNorm = 65,
    R1_UNorm = 66,
    R9G9B9E5_SharedExp = 67,
    R8G8_B8G8_UNorm = 68,
    G8R8_G8B8_UNorm = 69,
    BC1_TypeLess = 70,
    BC1_UNorm = 71,
    BC1_UNorm_sRGB = 72,
    BC2_TypeLess = 73,
    BC2_UNorm = 74,
    BC2_UNorm_sRGB = 75,
    BC3_TypeLess = 76,
    BC3_UNorm = 77,
    BC3_UNorm_sRGB = 78,
    BC4_TypeLess = 79,
    BC4_UNorm = 80,
    BC4_SNorm = 81,
    BC5_TypeLess = 82,
    BC5_UNorm = 83,
    BC5_SNorm = 84,
    B5G6R5_UNorm = 85,
    B5G5R5A1_UNorm = 86,
    B8G8R8A8_UNorm = 87,
    B8G8R8X8_UNorm = 88,
    R10G10B10_XR_Bias_A2_UNorm = 89,
    B8G8R8A8_TypeLess = 90,
    B8G8R8A8_UNorm_sRGB = 91,
    B8G8R8X8_TypeLess = 92,
    B8G8R8X8_UNorm_sRGB = 93,
    BC6H_TypeLess = 94,
    BC6H_UF16 = 95,
    BC6H_SF16 = 96,
    BC7_TypeLess = 97,
    BC7_UNorm = 98,
    BC7_UNorm_sRGB = 99,
}

public static partial class GpuFormatEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static FGpuTextureFormat ToFFI(this GpuTextureFormat format) => format switch
    {
        GpuTextureFormat.Unknown => FGpuTextureFormat.Unknown,
        GpuTextureFormat.R32G32B32A32_TypeLess => FGpuTextureFormat.R32G32B32A32_TypeLess,
        GpuTextureFormat.R32G32B32A32_Float => FGpuTextureFormat.R32G32B32A32_Float,
        GpuTextureFormat.R32G32B32A32_UInt => FGpuTextureFormat.R32G32B32A32_UInt,
        GpuTextureFormat.R32G32B32A32_SInt => FGpuTextureFormat.R32G32B32A32_SInt,
        GpuTextureFormat.R32G32B32_TypeLess => FGpuTextureFormat.R32G32B32_TypeLess,
        GpuTextureFormat.R32G32B32_Float => FGpuTextureFormat.R32G32B32_Float,
        GpuTextureFormat.R32G32B32_UInt => FGpuTextureFormat.R32G32B32_UInt,
        GpuTextureFormat.R32G32B32_SInt => FGpuTextureFormat.R32G32B32_SInt,
        GpuTextureFormat.R16G16B16A16_TypeLess => FGpuTextureFormat.R16G16B16A16_TypeLess,
        GpuTextureFormat.R16G16B16A16_Float => FGpuTextureFormat.R16G16B16A16_Float,
        GpuTextureFormat.R16G16B16A16_UNorm => FGpuTextureFormat.R16G16B16A16_UNorm,
        GpuTextureFormat.R16G16B16A16_UInt => FGpuTextureFormat.R16G16B16A16_UInt,
        GpuTextureFormat.R16G16B16A16_SNorm => FGpuTextureFormat.R16G16B16A16_SNorm,
        GpuTextureFormat.R16G16B16A16_SInt => FGpuTextureFormat.R16G16B16A16_SInt,
        GpuTextureFormat.R32G32_TypeLess => FGpuTextureFormat.R32G32_TypeLess,
        GpuTextureFormat.R32G32_Float => FGpuTextureFormat.R32G32_Float,
        GpuTextureFormat.R32G32_UInt => FGpuTextureFormat.R32G32_UInt,
        GpuTextureFormat.R32G32_SInt => FGpuTextureFormat.R32G32_SInt,
        GpuTextureFormat.R32G8X24_TypeLess => FGpuTextureFormat.R32G8X24_TypeLess,
        GpuTextureFormat.D32_Float_S8X24_UInt => FGpuTextureFormat.D32_Float_S8X24_UInt,
        GpuTextureFormat.R32_Float_X8X24_TypeLess => FGpuTextureFormat.R32_Float_X8X24_TypeLess,
        GpuTextureFormat.X32_TypeLess_G8X24_Float => FGpuTextureFormat.X32_TypeLess_G8X24_Float,
        GpuTextureFormat.R10G10B10A2_TypeLess => FGpuTextureFormat.R10G10B10A2_TypeLess,
        GpuTextureFormat.R10G10B10A2_UNorm => FGpuTextureFormat.R10G10B10A2_UNorm,
        GpuTextureFormat.R10G10B10A2_UInt => FGpuTextureFormat.R10G10B10A2_UInt,
        GpuTextureFormat.R11G11B10_Float => FGpuTextureFormat.R11G11B10_Float,
        GpuTextureFormat.R8G8B8A8_TypeLess => FGpuTextureFormat.R8G8B8A8_TypeLess,
        GpuTextureFormat.R8G8B8A8_UNorm => FGpuTextureFormat.R8G8B8A8_UNorm,
        GpuTextureFormat.R8G8B8A8_UNorm_sRGB => FGpuTextureFormat.R8G8B8A8_UNorm_sRGB,
        GpuTextureFormat.R8G8B8A8_UInt => FGpuTextureFormat.R8G8B8A8_UInt,
        GpuTextureFormat.R8G8B8A8_SNorm => FGpuTextureFormat.R8G8B8A8_SNorm,
        GpuTextureFormat.R8G8B8A8_SInt => FGpuTextureFormat.R8G8B8A8_SInt,
        GpuTextureFormat.R16G16_TypeLess => FGpuTextureFormat.R16G16_TypeLess,
        GpuTextureFormat.R16G16_Float => FGpuTextureFormat.R16G16_Float,
        GpuTextureFormat.R16G16_UNorm => FGpuTextureFormat.R16G16_UNorm,
        GpuTextureFormat.R16G16_UInt => FGpuTextureFormat.R16G16_UInt,
        GpuTextureFormat.R16G16_SNorm => FGpuTextureFormat.R16G16_SNorm,
        GpuTextureFormat.R16G16_SInt => FGpuTextureFormat.R16G16_SInt,
        GpuTextureFormat.R32_TypeLess => FGpuTextureFormat.R32_TypeLess,
        GpuTextureFormat.D32_Float => FGpuTextureFormat.D32_Float,
        GpuTextureFormat.R32_Float => FGpuTextureFormat.R32_Float,
        GpuTextureFormat.R32_UInt => FGpuTextureFormat.R32_UInt,
        GpuTextureFormat.R32_SInt => FGpuTextureFormat.R32_SInt,
        GpuTextureFormat.R24G8_TypeLess => FGpuTextureFormat.R24G8_TypeLess,
        GpuTextureFormat.D24_UNorm_S8_UInt => FGpuTextureFormat.D24_UNorm_S8_UInt,
        GpuTextureFormat.R24_UNorm_X8_TypeLess => FGpuTextureFormat.R24_UNorm_X8_TypeLess,
        GpuTextureFormat.X24_TypeLess_G8_UInt => FGpuTextureFormat.X24_TypeLess_G8_UInt,
        GpuTextureFormat.R8G8_TypeLess => FGpuTextureFormat.R8G8_TypeLess,
        GpuTextureFormat.R8G8_UNorm => FGpuTextureFormat.R8G8_UNorm,
        GpuTextureFormat.R8G8_UInt => FGpuTextureFormat.R8G8_UInt,
        GpuTextureFormat.R8G8_SNorm => FGpuTextureFormat.R8G8_SNorm,
        GpuTextureFormat.R8G8_SInt => FGpuTextureFormat.R8G8_SInt,
        GpuTextureFormat.R16_TypeLess => FGpuTextureFormat.R16_TypeLess,
        GpuTextureFormat.R16_Float => FGpuTextureFormat.R16_Float,
        GpuTextureFormat.D16_UNorm => FGpuTextureFormat.D16_UNorm,
        GpuTextureFormat.R16_UNorm => FGpuTextureFormat.R16_UNorm,
        GpuTextureFormat.R16_UInt => FGpuTextureFormat.R16_UInt,
        GpuTextureFormat.R16_SNorm => FGpuTextureFormat.R16_SNorm,
        GpuTextureFormat.R16_SInt => FGpuTextureFormat.R16_SInt,
        GpuTextureFormat.R8_TypeLess => FGpuTextureFormat.R8_TypeLess,
        GpuTextureFormat.R8_UNorm => FGpuTextureFormat.R8_UNorm,
        GpuTextureFormat.R8_UInt => FGpuTextureFormat.R8_UInt,
        GpuTextureFormat.R8_SNorm => FGpuTextureFormat.R8_SNorm,
        GpuTextureFormat.R8_SInt => FGpuTextureFormat.R8_SInt,
        GpuTextureFormat.A8_UNorm => FGpuTextureFormat.A8_UNorm,
        GpuTextureFormat.R1_UNorm => FGpuTextureFormat.R1_UNorm,
        GpuTextureFormat.R9G9B9E5_SharedExp => FGpuTextureFormat.R9G9B9E5_SharedExp,
        GpuTextureFormat.R8G8_B8G8_UNorm => FGpuTextureFormat.R8G8_B8G8_UNorm,
        GpuTextureFormat.G8R8_G8B8_UNorm => FGpuTextureFormat.G8R8_G8B8_UNorm,
        GpuTextureFormat.BC1_TypeLess => FGpuTextureFormat.BC1_TypeLess,
        GpuTextureFormat.BC1_UNorm => FGpuTextureFormat.BC1_UNorm,
        GpuTextureFormat.BC1_UNorm_sRGB => FGpuTextureFormat.BC1_UNorm_sRGB,
        GpuTextureFormat.BC2_TypeLess => FGpuTextureFormat.BC2_TypeLess,
        GpuTextureFormat.BC2_UNorm => FGpuTextureFormat.BC2_UNorm,
        GpuTextureFormat.BC2_UNorm_sRGB => FGpuTextureFormat.BC2_UNorm_sRGB,
        GpuTextureFormat.BC3_TypeLess => FGpuTextureFormat.BC3_TypeLess,
        GpuTextureFormat.BC3_UNorm => FGpuTextureFormat.BC3_UNorm,
        GpuTextureFormat.BC3_UNorm_sRGB => FGpuTextureFormat.BC3_UNorm_sRGB,
        GpuTextureFormat.BC4_TypeLess => FGpuTextureFormat.BC4_TypeLess,
        GpuTextureFormat.BC4_UNorm => FGpuTextureFormat.BC4_UNorm,
        GpuTextureFormat.BC4_SNorm => FGpuTextureFormat.BC4_SNorm,
        GpuTextureFormat.BC5_TypeLess => FGpuTextureFormat.BC5_TypeLess,
        GpuTextureFormat.BC5_UNorm => FGpuTextureFormat.BC5_UNorm,
        GpuTextureFormat.BC5_SNorm => FGpuTextureFormat.BC5_SNorm,
        GpuTextureFormat.B5G6R5_UNorm => FGpuTextureFormat.B5G6R5_UNorm,
        GpuTextureFormat.B5G5R5A1_UNorm => FGpuTextureFormat.B5G5R5A1_UNorm,
        GpuTextureFormat.B8G8R8A8_UNorm => FGpuTextureFormat.B8G8R8A8_UNorm,
        GpuTextureFormat.B8G8R8X8_UNorm => FGpuTextureFormat.B8G8R8X8_UNorm,
        GpuTextureFormat.R10G10B10_XR_Bias_A2_UNorm => FGpuTextureFormat.R10G10B10_XR_Bias_A2_UNorm,
        GpuTextureFormat.B8G8R8A8_TypeLess => FGpuTextureFormat.B8G8R8A8_TypeLess,
        GpuTextureFormat.B8G8R8A8_UNorm_sRGB => FGpuTextureFormat.B8G8R8A8_UNorm_sRGB,
        GpuTextureFormat.B8G8R8X8_TypeLess => FGpuTextureFormat.B8G8R8X8_TypeLess,
        GpuTextureFormat.B8G8R8X8_UNorm_sRGB => FGpuTextureFormat.B8G8R8X8_UNorm_sRGB,
        GpuTextureFormat.BC6H_TypeLess => FGpuTextureFormat.BC6H_TypeLess,
        GpuTextureFormat.BC6H_UF16 => FGpuTextureFormat.BC6H_UF16,
        GpuTextureFormat.BC6H_SF16 => FGpuTextureFormat.BC6H_SF16,
        GpuTextureFormat.BC7_TypeLess => FGpuTextureFormat.BC7_TypeLess,
        GpuTextureFormat.BC7_UNorm => FGpuTextureFormat.BC7_UNorm,
        GpuTextureFormat.BC7_UNorm_sRGB => FGpuTextureFormat.BC7_UNorm_sRGB,
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
    };
}

public readonly struct GpuTextureFormats : IEquatable<GpuTextureFormats>
{
    #region Fields

    private readonly GpuTextureFormat8 m_inner;
    private readonly int m_len;

    #endregion

    #region Props

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_len;
    }

    #endregion

    #region Ctor
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(GpuTextureFormat format0)
    {
        m_inner[0] = format0;
        m_len = 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(GpuTextureFormat format0, GpuTextureFormat format1)
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_len = 2;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2)
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_len = 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(
        GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2, GpuTextureFormat format3
    )
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_inner[3] = format3;
        m_len = 4;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(
        GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2,
        GpuTextureFormat format3, GpuTextureFormat format4
    )
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_inner[3] = format3;
        m_inner[4] = format4;
        m_len = 5;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(
        GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2,
        GpuTextureFormat format3, GpuTextureFormat format4, GpuTextureFormat format5
    )
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_inner[3] = format3;
        m_inner[4] = format4;
        m_inner[5] = format5;
        m_len = 6;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(
        GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2,
        GpuTextureFormat format3, GpuTextureFormat format4, GpuTextureFormat format5, GpuTextureFormat format6
    )
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_inner[3] = format3;
        m_inner[4] = format4;
        m_inner[5] = format5;
        m_inner[6] = format6;
        m_len = 7;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GpuTextureFormats(
        GpuTextureFormat format0, GpuTextureFormat format1, GpuTextureFormat format2, GpuTextureFormat format3,
        GpuTextureFormat format4, GpuTextureFormat format5, GpuTextureFormat format6, GpuTextureFormat format7
    )
    {
        m_inner[0] = format0;
        m_inner[1] = format1;
        m_inner[2] = format2;
        m_inner[3] = format3;
        m_inner[4] = format4;
        m_inner[5] = format5;
        m_inner[6] = format6;
        m_inner[7] = format7;
        m_len = 8;
    }

    #endregion

    #region AsSpan

    [UnscopedRef]
    public ReadOnlySpan<GpuTextureFormat> AsSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<GpuTextureFormat> s = m_inner;
            return s.Slice(0, m_len);
        }
    }

    #endregion

    #region Equals

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(GpuTextureFormats other) => AsSpan.SequenceEqual(other.AsSpan);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is GpuTextureFormats other && Equals(other);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(m_len);
        foreach (var format in AsSpan)
        {
            hash.Add(format);
        }
        return hash.ToHashCode();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(GpuTextureFormats left, GpuTextureFormats right) => left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(GpuTextureFormats left, GpuTextureFormats right) => !left.Equals(right);

    #endregion

    #region InlineArray

    [InlineArray(8)]
    private struct GpuTextureFormat8
    {
        private GpuTextureFormat _;
    }

    #endregion
}
