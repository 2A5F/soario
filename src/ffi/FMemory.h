#pragma once
#include <cstdint>

namespace ccc
{
    __declspec(dllexport) void* alloc(size_t size) noexcept;

    __declspec(dllexport) void free(void* ptr) noexcept;
}
