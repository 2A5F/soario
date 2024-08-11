#pragma once
#include "FGpuRt.h"
#include "FGpuPipelineState.h"

namespace ccc
{
    enum class FGpuCmdType
    {
        BarrierTransition = 1,
        ClearRt,
        SetRt,
        ReadyRasterizer,
        DispatchMesh,
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

    struct FGpuCmdSetRt
    {
        FGpuCmdType type;
        // todo 拆分 rtv 和 dsv
        FGpuRt* depth;
        /* 可以尾随 n 个 FGpuRt* */
        int32_t len;
    };

    struct FGpuCmdRasterizerViewPort
    {
        // 左上角原点 x y w h
        FFloat4 rect;
        // min max
        FFloat2 depth_range;
    };

    struct FGpuCmdRasterizerInfo
    {
        FInt4 scissor_rect;
        FGpuCmdRasterizerViewPort view_port;
    };

    struct FGpuCmdReadyRasterizer
    {
        FGpuCmdType type;
        /* 可以尾随 n 个 FGpuCmdRasterizerInfo */
        int32_t len;
    };

    struct FGpuCmdDispatchMesh
    {
        FGpuCmdType type;
        FUInt3 thread_groups;
        FGpuPipelineState* pipeline;
    };

    struct FGpuCmdList
    {
        uint8_t* datas;
        int32_t* indexes;
        size_t len;
    };
} // ccc
