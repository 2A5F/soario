#pragma once

#include "../FFI.h"

namespace ccc
{
    struct FGpuDeviceCreateOptions;
    struct FGpuDevice;

    struct FGpu : FObject
    {
        virtual FGpuDevice* CreateDevice(const FGpuDeviceCreateOptions& options, FError& err) = 0;
    };
}
