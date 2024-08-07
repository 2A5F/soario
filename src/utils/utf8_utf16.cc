#include "utf8_utf16.h"

namespace ccc
{
    Rc<String> utf16_to_utf8(const std::wstring_view& str)
    {
        return Rc(
            static_cast<String*>(app_fn_vtb().utf16_to_string8(
                {reinterpret_cast<const uint16_t*>(str.data()), str.size()}
            ))
        );
    }
}
