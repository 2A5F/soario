#pragma once
#include <atomic>
#include <winrt/base.h>

#include "../ffi/FFI.h"

namespace ccc
{
    struct Object
    {
        virtual ~Object() = default;
    };
} // ccc
