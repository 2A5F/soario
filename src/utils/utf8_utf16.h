#pragma once

#include "String.h"
#include "../App.h"

namespace ccc
{
    Rc<String> utf16_to_utf8(const std::wstring_view& str);
}
