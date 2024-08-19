#pragma once
#include "../FFI.h"

namespace ccc
{
    enum class FGpuResState
    {
        Common,
        RenderTarget,
    };

    struct FGpuRes : FObject
    {
        // ID3D12Resource*
        virtual void* get_res_raw_ptr() noexcept = 0;
    };
} // ccc
