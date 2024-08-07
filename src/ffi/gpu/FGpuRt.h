#pragma once

#include "../FFI.h"

namespace ccc
{
    struct FGpuRt : FObject
    {
        virtual FInt2 get_size() const noexcept = 0;
    };
} // ccc
