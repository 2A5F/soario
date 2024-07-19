#pragma once

#include "FFI.h"

namespace ccc
{
    struct WindowCreateOptions
    {
        FrStr16 title;
        FInt2 size;
        FInt2 min_size;
        FBool has_min_size;
    };

    enum class FWindowEventType
    {
        // 无事件，忽略
        None = 0,
        // 无数据
        Close = 1,
        // 无数据
        Resize = 2,
    };

    using fn_action__voidp_FWindowEventType_voidp = void(void* gc_handle, FWindowEventType type, void* data);

    struct FWindow : FObject
    {
        __declspec(dllexport) static FWindow* create(FError& err, const WindowCreateOptions& options) noexcept;

        virtual void set_gc_handle(void* handle) noexcept = 0;

        virtual void get_size(FError* err, FInt2* size) const noexcept = 0;
    };
} // ccc
