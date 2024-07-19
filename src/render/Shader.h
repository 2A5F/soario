#pragma once
#include "Gpu.h"
#include "../ffi/gpu/FShader.h"
#include "../utils/Rc.h"

namespace ccc
{
    struct ShaderModule final
    {
        // shader 二进制
        std::vector<uint8_t> m_blob;

        // todo 自己搞反射格式，给 dxc 抽出去
        winrt::com_ptr<ID3D12ShaderReflection> m_reflection;

        explicit ShaderModule(const Gpu& gpu, const FShaderStageData& stage_data);
    };

    enum class ShaderModuleFlags
    {
        // 异常值
        None = 0,

        Cs = 1 << 0,
        Ps = 1 << 1,
        Vs = 1 << 2,
        Ms = 1 << 3,
        As = 1 << 4,

        PsVs   = Ps | Vs,
        PsMs   = Ps | Ms,
        PsMsAs = PsMs | As,
    };

    class ShaderPass final : public FShaderPass
    {
        IMPL_RC(ShaderPass)

        // 着色器模块数据 ; cs | ps vs | ps ms as?
        std::shared_ptr<ShaderModule> m_shader_module[3];
        ShaderModuleFlags m_shader_module_flags;

        static Rc<ShaderPass> load_ms(FError& err, const Gpu& gpu, const FShaderPassData& pass_data);

    public:
        static Rc<ShaderPass> load(FError& err, const Gpu& gpu, const FShaderPassData& pass_data);
    };
} // ccc
