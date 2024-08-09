#pragma once

#include "../FFI.h"
#include "../FWindow.h"

namespace ccc
{
    struct FGpuConsts
    {
        static constexpr uint32_t FrameCount = 3;
    };

    struct FGpuSurface;

    struct FGpuDeviceCreateOptions;
    struct FGpuDevice;

    struct FGpu : FObject
    {
        FFI_EXPORT static FGpu* CreateGpu(FError& err) noexcept;

        virtual FGpuDevice* CreateDevice(const FGpuDeviceCreateOptions& options, FError& err) noexcept = 0;
    };
}
