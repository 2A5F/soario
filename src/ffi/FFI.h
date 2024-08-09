#pragma once
#include <stdint.h>

#define FFI_EXPORT __declspec(dllexport)

namespace ccc
{
    FFI_EXPORT void exit(int code) noexcept;

    struct FObject
    {
        virtual ~FObject() = default;

        virtual size_t AddRef() const noexcept = 0;

        virtual size_t Release() noexcept = 0;

        virtual size_t AddRefWeak() const noexcept = 0;

        virtual size_t ReleaseWeak() noexcept = 0;

        virtual bool TryDowngrade() noexcept = 0;

        virtual bool TryUpgrade() noexcept = 0;
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

    __declspec(align(8))
    struct FInt2
    {
        int32_t X;
        int32_t Y;
    };

    __declspec(align(16))
    struct FInt3
    {
        int32_t X;
        int32_t Y;
        int32_t Z;

    private:
        int32_t _pad{};
    };

    __declspec(align(16))
    struct FInt4
    {
        int32_t X;
        int32_t Y;
        int32_t Z;
        int32_t W;
    };

    __declspec(align(8))
    struct FFloat2
    {
        int32_t X;
        int32_t Y;
    };

    __declspec(align(16))
    struct FFloat3
    {
        float X;
        float Y;
        float Z;

    private:
        float _pad{};
    };

    __declspec(align(16))
    struct FFloat4
    {
        float X;
        float Y;
        float Z;
        float W;
    };

    __declspec(align(16))
    struct FFloatRect
    {
        float X;
        float Y;
        float W;
        float H;
    };

    using FBool = int32_t;

    enum class FErrorType
    {
        // 无错误
        None,
        // 一般错误
        Common,
        // Sdl utf8 c 字符串错误消息
        Sdl,
        // HResult FrStr16 字符串错误消息
        HResult,
        // 图形错误
        Gpu,
    };

    enum class FErrorMsgType
    {
        // utf8 c 字符串
        Utf8c,
        // utf16 c 字符串
        Utf16c,
        // FrStr8 字符串
        Utf8s,
        // FrStr16
        Utf16s,
    };

    union FErrorMsg
    {
        const char* u8c;
        const wchar_t* u16c;
        FrStr8 u8s;
        FrStr16 u16s;
    };

    struct FError
    {
        FErrorType type;
        FErrorMsgType msg_type;
        FErrorMsg msg;
        // 额外数据
        uint64_t data;
    };
}
