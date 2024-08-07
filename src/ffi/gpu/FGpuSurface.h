#pragma once
#include "FGpuRt.h"

namespace ccc
{
    struct FGpuSurfaceCreateOptions
    {
        FrStr16 name;

        bool v_sync;
    };

    struct FGpuSurface : FGpuRt
    {
        virtual bool get_v_sync() const noexcept = 0;

        virtual void set_v_sync(bool v) noexcept = 0;
    };
} // ccc
