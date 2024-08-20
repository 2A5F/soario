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
        Cbv,
    };

    struct FGpuViewCreateOptions
    {
        FGpuViewType type;
    };

    struct FGpuView : FObject
    {
        virtual FGpuViewType type() const noexcept = 0;
    };
} // ccc
