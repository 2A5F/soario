#include "FGpuDevice.h"

namespace ccc
{
    FGpuSurface* FGpuDevice::CreateSurfaceFromWindow(
        FGpuQueue* queue, const FGpuSurfaceCreateOptions& options, FWindow* window, FError& err
    ) noexcept
    {
        const auto hwnd = window->get_hwnd();
        return CreateSurfaceFromHwnd(queue, options, hwnd, err);
    }
} // ccc
