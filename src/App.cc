#include "App.h"

#include "pch.h"
#include "utils/utils.h"
#include "time.h"
#include "utils/dotnet.h"

namespace ccc
{
    namespace
    {
        AppFnVtb s_app_fn_vtb{};
        AppVars s_app_vars{};
    }

    AppFnVtb& app_fn_vtb()
    {
        return s_app_fn_vtb;
    }

    AppVars& app_vars()
    {
        return s_app_vars;
    }
} // ccc
