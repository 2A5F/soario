#pragma once
#include "windows.h"
#include "../ffi/gpu/FGpu.h"
#include "../ffi/gpu/FGpuSurface.h"

namespace ccc
{
    class GpuSurface : public FGpuSurface
    {
    public:
        static constexpr UINT FrameCount = FGpuConsts::FrameCount;
    };
} // ccc
