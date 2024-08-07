#pragma once

#include "FFI.h"

namespace ccc
{
    struct FString8 : FObject
    {
        const uint8_t* m_ptr;
        const size_t m_len;

        explicit FString8(const uint8_t* ptr, const size_t len) : m_ptr(ptr), m_len(len)
        {
        };

        __declspec(dllexport) static FString8* Create(FrStr8 slice) noexcept;
    };
} // ccc
