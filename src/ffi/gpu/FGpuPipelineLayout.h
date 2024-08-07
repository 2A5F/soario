#pragma once
#include "../FFI.h"

namespace ccc
{
    struct FGpuBindLessPipelineLayoutCreateOptions
    {
        FrStr16 name;
    };

    struct FGpuPipelineLayout : FObject
    {
        virtual void* get_raw_ptr() const noexcept = 0;
    };
} // ccc
