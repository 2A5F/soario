#pragma once
#include <string>

namespace ccc
{
    struct Args final
    {
        std::string exe_path;
        bool debug;

        static void set(Args args);

        static const Args& get();
    };
} // ccc
