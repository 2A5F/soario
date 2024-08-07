#pragma once

#include "FGpuCmd.h"
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
        virtual void submit(const FGpuCmdList* cmd_list, FError& err) noexcept = 0;
    };
} // ccc
