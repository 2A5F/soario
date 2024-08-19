#include "GpuView.h"

#include "../utils/Err.h"
#include "../utils/logger.h"

namespace ccc
{
    GpuView::GpuView(
        Rc<GpuResource> resource, const FGpuViewCreateOptions& options, FError& err
    ) : m_type(options.type), m_resource(std::move(resource))
    {
    }

    Rc<GpuView> GpuView::Create(Rc<GpuResource> resource, const FGpuViewCreateOptions& options, FError& err)
    {
        try
        {
            return Rc(new GpuView(std::move(resource), options, err));
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to create buffer!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }

    FGpuViewType GpuView::type() const noexcept
    {
        return m_type;
    }
} // ccc
