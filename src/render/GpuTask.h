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

        enum class State
        {
            Closed,
            Started,
            Ended,
        };

        Rc<GpuDevice> m_device;
        Rc<GpuQueue> m_queue;
        GpuFencePak m_fence_pak;

        State m_state{State::Closed};

        com_ptr<ID3D12Device2> m_dx_device{};
        com_ptr<ID3D12CommandQueue> m_command_queue{};

        com_ptr<ID3D12CommandAllocator> m_command_allocators{};
        com_ptr<ID3D12GraphicsCommandList6> m_command_list{};

        explicit GpuTask(Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err);

        void do_start();

        void start_inner(FError& err);

        void do_end();

        void submit_inner(const FGpuCmdList* cmd_list, FError& err);

        void wait_reset_inner();

        void wait_reset_async_inner(void* obj, fn_action__voidp cb);

        void do_reset();

        void wait_reset_any_what();

        void ensure_ended();

    public:
        ~GpuTask() override;

        static Rc<GpuTask> Create(
            Rc<GpuDevice> device, Rc<GpuQueue> queue, const FGpuTaskCreateOptions& options, FError& err
        ) noexcept;

        void start(FError& err) noexcept override;

        void submit(const FGpuCmdList* cmd_list, FError& err) noexcept override;

        void end(FError& err) noexcept override;

        void wait_reset(FError& err) noexcept override;

        void wait_reset_async(FError& err, void* obj, fn_action__voidp cb) noexcept override;
    };
} // ccc
