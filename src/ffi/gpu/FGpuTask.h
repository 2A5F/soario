#pragma once

#include "FGpuCmd.h"
#include "../FFI.h"
#include "../FnPtrs.h"

namespace ccc
{
    struct FGpuTaskCreateOptions
    {
        FrStr16 name;
    };

    struct FGpuTask : FObject
    {
        /* 开始任务 */
        virtual void start(FError& err) noexcept = 0;

        /* 提交命令 */
        virtual void submit(const FGpuCmdList* cmd_list, FError& err) noexcept = 0;

        /* 结束任务 */
        virtual void end(FError& err) noexcept = 0;

        /* 等待任务可重用 */
        virtual void wait_reset(FError& err) noexcept = 0;

        /* 等待任务可重用 */
        virtual void wait_reset_async(FError& err, void* obj, fn_action__voidp cb) noexcept = 0;
    };
} // ccc
