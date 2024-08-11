#pragma once
#include <cstdint>

#include "FFI.h"
#include "FLogLevel.h"

#define FFI_CDECL __cdecl
#define FFI_STDCALL __stdcall

namespace ccc
{
    struct FString8;

    using fn_action = void FFI_CDECL();

    using fn_action__voidp = void FFI_CDECL(void*);

    using fn_action__voidp_voidp = void FFI_CDECL(void*, void*);

    using fn_func__FrStr16__size_t = size_t FFI_CDECL(FrStr16);

    using fn_func__FrStr16_FmStr8__size_t = size_t FFI_CDECL(FrStr16, FmStr8);

    using fn_func__FrStr16__FString8p = FString8* FFI_CDECL(FrStr16);

    using fn_func__FLogLevel_charp__void = void FFI_CDECL(FLogLevel, const char*);
    using fn_func__FLogLevel_wcharp__void = void FFI_CDECL(FLogLevel, const wchar_t*);
    using fn_func__FLogLevel_FrStr8__void = void FFI_CDECL(FLogLevel, FrStr8);
    using fn_func__FLogLevel_FrStr16__void = void FFI_CDECL(FLogLevel, FrStr16);

    using fn_func__voidp_FLogLevel_charp__void = void FFI_CDECL(void*, FLogLevel, const char*);

    using fn_func__voidp__void = void FFI_CDECL(void*);
}
