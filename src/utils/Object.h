#pragma once
#include <atomic>
#include <winrt/base.h>

#include "../api/Api.h"

namespace ccc
{
    struct Object
    {
        virtual ~Object() = default;
    };
} // ccc
