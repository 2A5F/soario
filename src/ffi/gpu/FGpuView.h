#pragma once
#include "../FFI.h"

namespace ccc
{
    enum class FGpuViewType
    {
        Unknown,
        Rtv,
        Dsv,
        Uav,
        Srv,
    };

    struct FGpuViewCreateOptions
    {
        FrStr16 name;
        FGpuViewType type;
    };

    struct FGpuView : FObject
    {
        virtual FGpuViewType type() const noexcept = 0;
    };
} // ccc
