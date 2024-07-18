#include "utf8_utf16.h"

namespace ccc {
    String utf16_to_utf8(const std::wstring_view &str) {
        const FrStr16 s16{reinterpret_cast<const uint16_t *>(str.data()), str.length()};
        const auto max_len = app_fn_vtb().p_fn_utf16_get_utf8_max_len(s16);
        const auto ptr = new uint8_t[max_len];
        const FmStr8 s8{ptr, max_len};
        auto const len = app_fn_vtb().p_fn_utf16_to_utf8(s16, s8);
        return String(ptr, len);
    }
}
