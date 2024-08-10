#pragma once

#include "FGpuPipelineLayout.h"
#include "FGpuPipelineState.h"
#include "../FFI.h"
#include "../FnPtrs.h"
#include "../FWindow.h"

namespace ccc
{
    struct FGpuSurfaceCreateOptions;
    struct FGpuSurface;

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
        virtual FGpuSurface* CreateSurfaceFromHwnd(
            FGpuQueue* queue, const FGpuSurfaceCreateOptions& options, size_t hwnd, FError& err
        ) noexcept = 0;

        virtual FGpuSurface* CreateSurfaceFromWindow(
            FGpuQueue* queue, const FGpuSurfaceCreateOptions& options, FWindow* window, FError& err
        ) noexcept;

        virtual FGpuQueue* CreateQueue(const FGpuQueueCreateOptions& options, FError& err) noexcept = 0;

        virtual FGpuPipelineLayout* CreateBindLessPipelineLayout(
            const FGpuBindLessPipelineLayoutCreateOptions& options, FError& err
        ) noexcept = 0;

        virtual FGpuPipelineState* CreatePipelineState(
            FGpuPipelineLayout* layout, const FGpuPipelineStateCreateOptions& options, FError& err
        ) noexcept = 0;
    };
} // ccc
