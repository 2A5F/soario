#pragma once

#include "FGpu.h"
#include "../FFI.h"

namespace ccc
{
    struct FShaderStageData
    {
        FrStr8 blob;
        FrStr8 reflection;
    };

    struct FShaderPassData
    {
        FShaderStageData* Ps;
        FShaderStageData* Vs;
        FShaderStageData* Cs;
        FShaderStageData* Ms;
        FShaderStageData* As;
    };

    struct FShaderPass : FObject
    {
        __declspec(dllexport) static FShaderPass* load(FError& err, const FGpu& gpu, const FShaderPassData& pass_data);
    };
} // ccc
