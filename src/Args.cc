#include "Args.h"

#include <memory>

namespace ccc
{
    namespace
    {
        std::unique_ptr<Args> s_args;
    }

    void Args::set(const Args args)
    {
        if (s_args == nullptr) s_args = std::make_unique<Args>();
        *s_args = args;
    }

    const Args& Args::get()
    {
        return *s_args;
    }
} // ccc
