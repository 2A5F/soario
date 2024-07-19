#include "FShader.h"

#include "../../render/Shader.h"

namespace ccc
{
    FShaderPass* FShaderPass::load(FError& err, const FGpu& gpu, const FShaderPassData& pass_data)
    {
        auto sm = ShaderPass::load(err, *dynamic_cast<const Gpu*>(&gpu), pass_data);
        return sm.leak();
    }
} // ccc
