#pragma once

#include "../FFI.h"

namespace ccc
{
    enum class FGpuQueueType
    {
        Common,
        Compute,
        Copy,
    };

    struct FGpuQueueCreateOptions
    {
        FrStr16 name;
        FGpuQueueType type;
    };

    struct FGpuQueue : FObject
    {
    };
} // ccc
