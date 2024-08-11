#pragma once

#include "FGpuTask.h"
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
        virtual FGpuTask* CreateTask(
            const FGpuTaskCreateOptions& options, FError& err
        ) noexcept = 0;
    };
} // ccc
