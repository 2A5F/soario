#include "GpuQueue.h"

namespace ccc {
    GpuQueue::GpuQueue(
        com_ptr<ID3D12Device> device,
        const D3D12_COMMAND_LIST_TYPE type
    ): m_device(std::move(device)) {
        for (int i = 0; i < FrameCount; ++i) {
            winrt::check_hresult(
                m_device->CreateCommandAllocator(
                    D3D12_COMMAND_LIST_TYPE_DIRECT, RT_IID_PPV_ARGS(m_command_allocators[i])));
        }

        const D3D12_COMMAND_QUEUE_DESC queue_desc = {
            .Type = type,
            .Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
        };

        winrt::check_hresult(m_device->CreateCommandQueue(&queue_desc, RT_IID_PPV_ARGS(m_command_queue)));

        winrt::check_hresult(
                m_device->CreateCommandList(
                    0, D3D12_COMMAND_LIST_TYPE_DIRECT,
                    m_command_allocators[0].get(), nullptr, RT_IID_PPV_ARGS(m_command_list)));

        winrt::check_hresult(m_command_list->Close());
    }

    void GpuQueue::ready_frame(const int frame_index) const {
        winrt::check_hresult(m_command_allocators[frame_index]->Reset());
        winrt::check_hresult(m_command_list->Reset(m_command_allocators[frame_index].get(), nullptr));
    }
} // ccc
