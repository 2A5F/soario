#include "Shader.h"

#include "Gpu.h"
#include "../utils/Err.h"

namespace ccc
{
    ShaderModule::ShaderModule(const Gpu& gpu, const FShaderStageData& stage_data)
    {
        const auto& [b, r] = stage_data;
        const DxcBuffer refl{r.ptr, r.len, 0};
        winrt::check_hresult(gpu.m_dxc_utils->CreateReflection(&refl, RT_IID_PPV_ARGS(m_reflection)));

        m_blob = std::vector(b.ptr, b.ptr + b.len);
    }

    Rc<ShaderPass> ShaderPass::load(FError& err, const Gpu& gpu, const FShaderPassData& pass_data)
    {
        if (&gpu == nullptr)
        {
            err = make_error(FErrorType::Common, u"gpu is null");
            return nullptr;
        }

        if (pass_data.Cs != nullptr)
        {
            err = make_error(FErrorType::Common, u"cs is not supported yet");
            return nullptr;
        }

        if (pass_data.Ps == nullptr)
        {
            err = make_error(FErrorType::Common, u"Invalid pipeline");
            return nullptr;
        }

        if (pass_data.Vs != nullptr)
        {
            err = make_error(FErrorType::Common, u"vs is not supported yet");
            return nullptr;
        }

        if (pass_data.Ms != nullptr)
        {
            return load_ms(err, gpu, pass_data);
        }

        err = make_error(FErrorType::Common, u"Invalid pipeline");
        return nullptr;
    }

    Rc<ShaderPass> ShaderPass::load_ms(FError& err, const Gpu& gpu, const FShaderPassData& pass_data)
    {
        try
        {
            const auto& ps = *pass_data.Ps;
            const auto& ms = *pass_data.Ms;
            const auto& as = *pass_data.As;

            Rc sp(new ShaderPass());

            sp->m_shader_module[0] = std::make_shared<ShaderModule>(gpu, ps);
            sp->m_shader_module[1] = std::make_shared<ShaderModule>(gpu, ms);

            if (pass_data.As != nullptr)
            {
                sp->m_shader_module[2] = std::make_shared<ShaderModule>(gpu, as);
            }

            sp->m_shader_module_flags = pass_data.As == nullptr ? ShaderModuleFlags::PsMs : ShaderModuleFlags::PsMsAs;

            return sp;
        }
        catch (winrt::hresult_error ex)
        {
            err = make_hresult_error(ex);
            return nullptr;
        }
    }
} // ccc
