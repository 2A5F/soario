#pragma once
#include "GpuDevice.h"
#include "GpuFencePak.h"
#include "../ffi/gpu/FGpuTask.h"
#include "../utils/Rc.h"

namespace ccc
{
    class GpuSurfaceHwnd;

    class GpuTask final : public FGpuTask
    {
        IMPL_RC(GpuTask);

        friend class GpuSurfaceHwnd;

        Rc<GpuDevice> m_device;
        Rc<GpuQueue> m_queue;
        GpuFencePak m_fence_pak;

        bool m_closed{true};

        com_ptr<ID3D12Device2> m_dx_device{};
        com_ptr<ID3D12CommandQueue> m_command_queue{};

        com_ptr<ID3D12CommandAllocator> m_command_allocators{};
        com_ptr<ID3D12GraphicsCommandList6> m_command_list{};

        explicit GpuTask(Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err);

        void submit_inner(const FGpuCmdList* cmd_list, FError& err);

        void wait_reset_inner();

    public:
        static Rc<GpuTask> Create(
            Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err
        ) noexcept;

        void submit(const FGpuCmdList* cmd_list, FError& err) noexcept override;

        void end(FError& err) noexcept override;

        void wait_reset(FError& err) noexcept override;
    };
} // ccc
