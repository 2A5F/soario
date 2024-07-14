#include "GpuCommandList.h"

namespace ccc {
    GpuCommandList::GpuCommandList(
        com_ptr<ID3D12GraphicsCommandList> command_list
    ) : m_command_list(std::move(command_list)) {
    }

    void GpuCommandList::set_rt(std::shared_ptr<IRT> rt) {
        if (rt == nullptr) throw std::exception("rt is null");
        m_current_rt = std::move(rt);
        if (CD3DX12_RESOURCE_BARRIER barrier; m_current_rt->require_state(GpuRtState::RenderTarget, barrier)) {
            m_command_list->ResourceBarrier(1, &barrier);
        }
        const auto cpu_handle = m_current_rt->get_cpu_handle();
        m_command_list->OMSetRenderTargets(1, &cpu_handle, FALSE, nullptr);
    }

    void GpuCommandList::clear(const float4 color) {
        if (m_current_rt == nullptr) throw std::exception("rt is not set");
        clear(m_current_rt, color);
    }

    void GpuCommandList::clear(const std::shared_ptr<IRT> &rt, float4 color) {
        if (CD3DX12_RESOURCE_BARRIER barrier; rt->require_state(GpuRtState::RenderTarget, barrier)) {
            m_command_list->ResourceBarrier(1, &barrier);
        }
        const auto cpu_handle = rt->get_cpu_handle();
        m_command_list->ClearRenderTargetView(cpu_handle, &color.x, 0, nullptr);
    }
} // ccc
