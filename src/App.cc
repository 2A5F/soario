#include "App.h"

#include "pch.h"
#include "utils/utils.h"
#include "time.h"
#include "utils/dotnet.h"
#include "utils/Time.h"

namespace ccc
{
    namespace
    {
        AppFnVtb s_app_fn_vtb;
    }

    AppFnVtb& app_fn_vtb()
    {
        return s_app_fn_vtb;
    }
} // ccc
