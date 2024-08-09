#pragma once

#include "FFI.h"

namespace ccc
{
    FFI_EXPORT void* alloc(size_t size) noexcept;

    FFI_EXPORT void* realloc(void* old, size_t size) noexcept;

    FFI_EXPORT void free(void* ptr) noexcept;

    FFI_EXPORT FmStr8 alloc_str(size_t size) noexcept;

    FFI_EXPORT void free_str(FmStr8 str) noexcept;
}
