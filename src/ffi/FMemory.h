#pragma once
#include <cstdint>

#include "FFI.h"

namespace ccc
{
    __declspec(dllexport) void* alloc(size_t size) noexcept;

    __declspec(dllexport) void* realloc(void* old, size_t size) noexcept;

    __declspec(dllexport) void free(void* ptr) noexcept;

    __declspec(dllexport) FmStr8 alloc_str(size_t size) noexcept;

    __declspec(dllexport) void free_str(FmStr8 str) noexcept;
}
