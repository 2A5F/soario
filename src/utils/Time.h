#pragma once
#include <chrono>
#include <ctime>

namespace ccc {
    struct Time {
        std::chrono::steady_clock::time_point start_time;
        std::chrono::steady_clock::time_point last_time;
        std::chrono::steady_clock::time_point now_time;
        std::chrono::steady_clock::duration delta_time_raw;
        std::chrono::steady_clock::duration total_time_raw;
        double delta_time;
        double total_time;

        void init();

        void tick();

        double delta() const;

        double total() const;
    };

    namespace time {
        void init();

        void tick();

        std::chrono::steady_clock::duration delta_raw();

        std::chrono::steady_clock::duration total_raw();

        double delta();

        double total();
    }
} // ccc
