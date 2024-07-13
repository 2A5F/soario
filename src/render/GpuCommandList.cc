#include "GpuCommandList.h"

namespace ccc {
    GpuCommandList::GpuCommandList(
        com_ptr<ID3D12GraphicsCommandList> command_list
    ) : m_command_list(std::move(command_list)),
        m_rt_state(GpuCommandListRtState::None) {
    }

    void GpuCommandList::clear(float4 color) {
    }
} // ccc
