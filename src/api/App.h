#pragma once
#include "FnPtrs.h"

namespace ccc {
    struct AppFnVtb {
        fn_func__FrStr16__size_t* p_fn_utf16_get_utf8_max_len;
        fn_func__FrStr16_FmStr8__size_t* p_fn_utf16_to_utf8;

        fn_action* p_fn_start;
    };
}
