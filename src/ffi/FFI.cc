#include "FFI.h"

#include "../window/Window.h"

namespace ccc
{
    void exit(const int code) noexcept
    {
        WindowSystem::exit(code);
    }
}
