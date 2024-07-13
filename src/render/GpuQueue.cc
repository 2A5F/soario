#include "GpuQueue.h"

namespace ccc {
    GpuQueue::GpuQueue(
        com_ptr<ID3D12Device> device,
        com_ptr<D3D12MA::Allocator> gpu_allocator,
        com_ptr<ID3D12CommandAllocator> command_allocator,
        com_ptr<ID3D12CommandQueue> command_queue,
        com_ptr<ID3D12GraphicsCommandList> m_command_list
    ): m_device(std::move(device)),
       m_gpu_allocator(std::move(gpu_allocator)),
       m_command_allocator(std::move(command_allocator)),
       m_command_queue(std::move(command_queue)),
       m_command_list(std::move(m_command_list)) {
    }
} // ccc
