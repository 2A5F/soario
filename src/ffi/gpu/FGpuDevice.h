#pragma once

#include "../FFI.h"
#include "../FnPtrs.h"

namespace ccc
{
    struct FGpuQueue;
    struct FGpuQueueCreateOptions;

    struct FGpuDeviceCreateOptions
    {
        FrStr16 name;

        fn_func__voidp_FLogLevel_charp__void* logger;
        void* logger_object;
        /* logger_object 不为空时析构会调用此函数销毁 object，如果此函数为空将跳过 */
        fn_func__voidp__void* logger_drop_object;
    };

    struct FGpuDevice : FObject
    {
        virtual FGpuQueue* CreateQueue(const FGpuQueueCreateOptions& options, FError& err) noexcept = 0;
    };
} // ccc
