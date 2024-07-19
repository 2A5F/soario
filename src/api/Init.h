#pragma once
#include "App.h"
#include "Time.h"

namespace ccc
{
    struct InitParams
    {
        TimeData* p_time_data;
    };

    struct InitResult
    {
        AppFnVtb fn_vtb;
    };
}
