#pragma once
#include <stdint.h>
#include <atomic>

namespace ccc
{
    __declspec(dllexport) void exit(int code);

    struct FObject
    {
        virtual ~FObject() = default;

        virtual size_t AddRef() const = 0;

        virtual size_t Release() = 0;
    };

    struct FmStr16
    {
        uint16_t* ptr;
        size_t len;
    };

    struct FmStr8
    {
        uint8_t* ptr;
        size_t len;
    };

    struct FrStr16
    {
        const uint16_t* ptr;
        size_t len;
    };

    struct FrStr8
    {
        const uint8_t* ptr;
        size_t len;
    };

    __declspec(align(16))
    struct FInt2
    {
        int32_t X;
        int32_t Y;
    };

    using FBool = int32_t;
}
