#pragma once
#include "App.h"

namespace ccc
{
    struct InitParams
    {
        AppVars* p_vas;
    };

    struct InitResult
    {
        AppFnVtb fn_vtb;
    };
}

