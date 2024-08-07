#pragma once
#include <cstdint>

#include "FFI.h"
#include "FLogLevel.h"

namespace ccc
{
    struct FString8;

    using fn_action = void();

    using fn_func__FrStr16__size_t = size_t (FrStr16);

    using fn_func__FrStr16_FmStr8__size_t = size_t (FrStr16, FmStr8);

    using fn_func__FrStr16__FString8p = FString8* (FrStr16);

    using fn_func__FLogLevel_charp__void = void (FLogLevel, const char*);
    using fn_func__FLogLevel_wcharp__void = void (FLogLevel, const wchar_t*);
    using fn_func__FLogLevel_FrStr8__void = void (FLogLevel, FrStr8);
    using fn_func__FLogLevel_FrStr16__void = void (FLogLevel, FrStr16);

    using fn_func__voidp_FLogLevel_charp__void = void (void*, FLogLevel, const char*);

    using fn_func__voidp__void = void (void*);
}
