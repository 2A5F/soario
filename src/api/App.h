#pragma once
#include "FnPtrs.h"
#include "./FWindow.h"

namespace ccc
{
    struct AppFnVtb
    {
        fn_func__FrStr16__size_t* utf16_get_utf8_max_len;
        fn_func__FrStr16_FmStr8__size_t* utf16_to_utf8;

        fn_action* start;

        fn_action__voidp_FWindowEventType_voidp* window_event_handle;
    };
}
