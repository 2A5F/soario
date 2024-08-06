#include "FMemory.h"

#include <mimalloc.h>

void* ccc::alloc(const size_t size) noexcept
{
    return mi_malloc(size);
}

void* ccc::realloc(void* old, const size_t size) noexcept
{
    return mi_realloc(old, size);
}

void ccc::free(void* ptr) noexcept
{
    mi_free(ptr);
}

ccc::FmStr8 ccc::alloc_str(const size_t size) noexcept
{
    return {new uint8_t[size], size};
}

void ccc::free_str(const FmStr8 str) noexcept
{
    delete[] str.ptr;
}
