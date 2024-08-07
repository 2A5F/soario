#pragma once
#include "FGpuRt.h"

namespace ccc
{
    enum class FGpuCmdType
    {
        BarrierTransition = 1,
        ClearRtv,
    };

    struct FGpuCmdClearRtvFlag
    {
        uint8_t color   : 1;
        uint8_t depth   : 1;
        uint8_t stencil : 1;

        bool any() const { return color || depth || stencil; }
    };

    struct FGpuCmdClearRtv
    {
        FGpuCmdType type;
        FGpuCmdClearRtvFlag flag;
        FGpuRt* rt;
        FFloat4 color;
        int32_t rect_len;
        float depth;
        uint8_t stencil;
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
