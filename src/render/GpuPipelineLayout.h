#pragma once
#include "GpuDevice.h"

namespace ccc
{
    class GpuPipelineLayout : public FGpuPipelineLayout
    {
    public:
        virtual bool is_bind_less() const { return false; }

        virtual const com_ptr<ID3D12RootSignature>& get_root_signature() const = 0;
    };
} // ccc
