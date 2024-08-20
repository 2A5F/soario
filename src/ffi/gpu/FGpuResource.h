#pragma once
#include "FGpuFormat.h"
#include "FGpuRes.h"
#include "FGpuView.h"

namespace ccc
{
    enum class FGpuResourceDimension
    {
        Unknown,
        Buffer,
        Texture1D,
        Texture2D,
        Texture3D,
    };

    enum class FGpuResourceLayout
    {
        Unknown,
        RowMajor,
        UndefinedSwizzle64KB,
        StandardSwizzle64KB,
    };

    struct FGpuResourceFlags
    {
        uint32_t rtv                                : 1;
        uint32_t dsv                                : 1;
        uint32_t uav                                : 1;
        uint32_t srv                                : 1;
        uint32_t cross_gpu                          : 1;
        uint32_t shared_access                      : 1;
        uint32_t ray_tracing_acceleration_structure : 1;
    };

    struct FGpuResourceBufferInfo
    {
        FGpuResourceDimension dimension;
        int64_t align;
        int64_t size;
        FGpuResourceFlags flags;
    };

    struct FGpuResourceTextureInfo
    {
        FGpuResourceDimension dimension;
        int64_t align;
        int64_t width;
        int32_t height;
        int16_t depth_or_length;
        int16_t mip_levels;
        FGpuTextureFormat format;
        uint32_t sample_count;
        uint32_t sample_quality;
        FGpuResourceLayout layout;
        FGpuResourceFlags flags;
    };

    union FGpuResourceInfo
    {
        FGpuResourceDimension dimension;
        FGpuResourceBufferInfo buffer_info;
        FGpuResourceTextureInfo texture_info;
    };

    enum class FGpuResourceUsage
    {
        GpuOnly,
        CpuToGpu,
        GpuToCpu,
    };

    struct FGpuResourceBufferCreateOptions
    {
        FrStr16 name;
        FGpuResourceUsage usage;
        int64_t size;
        FGpuResourceFlags flags;
    };

    struct FGpuResourceTextureCreateOptions
    {
        FrStr16 name;
        FGpuResourceUsage usage;
        FGpuResourceDimension dimension;
        int64_t align;
        int64_t width;
        int32_t height;
        int16_t depth_or_length;
        int16_t mip_levels;
        FGpuTextureFormat format;
        uint32_t sample_count;
        uint32_t sample_quality;
        FGpuResourceLayout layout;
        FGpuResourceFlags flags;
    };

    struct FGpuResource : FGpuRes
    {
        virtual const FGpuResourceInfo* get_info() const noexcept = 0;

        virtual FGpuView* get_view(const FGpuViewCreateOptions& options, FError& err) noexcept = 0;
    };
} // ccc
