#pragma once

#include "FGpuCmd.h"
#include "../FFI.h"

namespace ccc
{
    struct FGpuTaskCreateOptions
    {
        FrStr16 name;
    };

    struct FGpuTask : FObject
    {
        /* 提交命令 */
        virtual void submit(const FGpuCmdList* cmd_list, FError& err) noexcept = 0;

        /* 结束任务 */
        virtual void end(FError& err) noexcept = 0;

        /* 等待任务可重用 */
        virtual void wait_reset(FError& err) noexcept = 0;
    };
} // ccc
