#pragma once
#include <chrono>
#include <ctime>
#include "../ffi/Time.h"

namespace ccc
{
    struct Time final
    {
        TimeData data;

        void init();

        void tick();

        double delta() const;

        double total() const;
    };

    namespace time
    {
        TimeData* get_data_ptr();

        void init();

        void tick();

        std::chrono::steady_clock::duration delta_raw();

        std::chrono::steady_clock::duration total_raw();

        double delta();

        double total();
    }
} // ccc
