#pragma once

#include "Api.h"

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
        __declspec(dllexport) static FWindow* create(const WindowCreateOptions& options);

        virtual void set_gc_handle(void* handle) = 0;
    };
} // ccc
