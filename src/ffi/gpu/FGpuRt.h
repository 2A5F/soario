#pragma once

#include "FGpuRes.h"
#include "../FFI.h"

namespace ccc
{
    struct FGpuRt : FGpuRes
    {
        virtual FInt2 get_size() const noexcept = 0;

        virtual bool has_rtv() noexcept = 0;

        virtual bool has_dsv() noexcept = 0;

        virtual size_t get_cpu_rtv_handle(FError& err) noexcept = 0;

        virtual size_t get_cpu_dsv_handle(FError& err) noexcept = 0;
    };
} // ccc
