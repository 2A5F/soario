#include "GpuView.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuView::GpuView(const FGpuViewType type) : m_type(type)
    {
    }

    FGpuViewType GpuView::type() const noexcept
    {
        return m_type;
    }
} // ccc
