#pragma once
#include "../ffi/FFI.h"

#include "SDL3/SDL.h"

namespace ccc
{
    inline FError make_error_ok()
    {
        FError err{};
        err.type = FErrorType::None;
        return err;
    }

    template <const size_t N>
    FError make_error(const FErrorType type, const char (&msg)[N])
    {
        FError err{};
        err.type = type;
        err.msg_type = FErrorMsgType::Utf8s;
        err.msg.u8s.ptr = reinterpret_cast<const uint8_t*>(&*msg);
        err.msg.u8s.len = N - 1;
        // ReSharper disable once CppSomeObjectMembersMightNotBeInitialized
        return err;
    }

    template <const size_t N>
    FError make_error(const FErrorType type, const char16_t (&msg)[N])
    {
        FError err{};
        err.type = type;
        err.msg_type = FErrorMsgType::Utf16s;
        err.msg.u16s.ptr = reinterpret_cast<const uint16_t*>(&*msg);
        err.msg.u16s.len = N - 1;
        // ReSharper disable once CppSomeObjectMembersMightNotBeInitialized
        return err;
    }

    inline FError make_error(const FErrorType type, const char* msg)
    {
        FError err{};
        err.type = type;
        err.msg_type = FErrorMsgType::Utf8c;
        err.msg.u8c = msg;
        // ReSharper disable once CppSomeObjectMembersMightNotBeInitialized
        return err;
    }

    inline FError make_error(const FErrorType type, const wchar_t* msg)
    {
        FError err{};
        err.type = type;
        err.msg_type = FErrorMsgType::Utf8c;
        err.msg.u16c = msg;
        // ReSharper disable once CppSomeObjectMembersMightNotBeInitialized
        return err;
    }

    inline FError make_sdl_error()
    {
        const auto msg = SDL_GetError();
        return make_error(FErrorType::Sdl, msg);
    }

    inline FError make_hresult_error(const winrt::hresult_error& err)
    {
        const auto& msg = err.message();
        FError e{};
        e.type = FErrorType::HResult;
        e.data = err.code();
        e.msg_type = FErrorMsgType::Utf16s;
        e.msg.u16s.ptr = reinterpret_cast<const uint16_t*>(msg.data());
        e.msg.u16s.len = msg.size();
        // ReSharper disable once CppSomeObjectMembersMightNotBeInitialized
        return e;
    }
}
