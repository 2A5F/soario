#include "FGpu.h"

#include "../../render/Gpu.h"
#include "../../utils/Err.h"
#include "../../utils/logger.h"

namespace ccc
{
    FGpu* FGpu::CreateGpu(FError& err) noexcept
    {
        try
        {
            Rc r(new Gpu);
            return r.leak();
        }
        catch (std::exception ex)
        {
            logger::error(ex.what());
            err = make_error(FErrorType::Gpu, u"Failed to create device!");
            return nullptr;
        }
        catch (winrt::hresult_error ex)
        {
            logger::error(ex.message());
            err = make_hresult_error(ex);
            return nullptr;
        }
    }
}
