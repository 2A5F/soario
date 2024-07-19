#include "FMemory.h"

#include <mimalloc.h>

void* ccc::alloc(size_t size) noexcept
{
    return mi_malloc(size);
}

void ccc::free(void* ptr) noexcept
{
    mi_free(ptr);
}
