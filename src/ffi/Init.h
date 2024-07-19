#pragma once
#include "App.h"
#include "Time.h"
#include "./gpu/FGpu.h"

namespace ccc
{
    struct InitParams
    {
        TimeData* p_time_data;
        FGpu* p_gpu;
    };

    struct InitResult
    {
        AppFnVtb fn_vtb;
    };
}

