#pragma once
#include "../FFI.h"

namespace ccc
{
    struct FGpuPipelineCreateFlag
    {
        uint8_t bind_less : 1;
        uint8_t cs        : 1;
        uint8_t ps        : 1;
        uint8_t vs        : 1;
        uint8_t ms        : 1;
        uint8_t as        : 1;
    };

    struct FGpuPipelineStateCreateOptions
    {
        FrStr16 name;
        FGpuPipelineCreateFlag flag;
        FrStr8 blob[3];
        // todo
    };

    struct FGpuPipelineState : FObject
    {
    };
} // ccc
