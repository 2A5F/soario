#pragma once
#include "FGpuRt.h"

namespace ccc
{
    enum class FGpuCmdType
    {
        BarrierTransition = 1,
        ClearRt,
    };

    struct FGpuCmdClearRtFlag
    {
        uint8_t color   : 1;
        uint8_t depth   : 1;
        uint8_t stencil : 1;

        bool any() const { return color || depth || stencil; }
    };

    struct FGpuCmdClearRt
    {
        FGpuCmdType type;
        /* 可以尾随 n 个 int4 rect */
        int32_t rect_len;
        FGpuRt* rtv;
        FGpuRt* dsv;
        FFloat4 color;
        float depth;
        uint8_t stencil;
        FGpuCmdClearRtFlag flag;
    };

    struct FGpuCmdBarrierTransition
    {
        FGpuCmdType type;
        uint32_t sub_res;
        FGpuRes* res;
        FGpuResState pre_state;
        FGpuResState cur_state;
    };

    struct FGpuCmdList
    {
        uint8_t* datas;
        int32_t* indexes;
        size_t len;
    };
} // ccc
