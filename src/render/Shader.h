#pragma once
#include "../utils/Object.h"
#include "../ffi/gpu/FShader.h"
#include "../utils/Rc.h"

namespace ccc
{
    class Shader final : public FShader
    {
        IMPL_RC(Shader)

    };
} // ccc
