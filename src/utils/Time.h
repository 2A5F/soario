#pragma once
#include <ctime>

namespace ccc {
    struct Time {
        clock_t start_time;
        clock_t last_time;
        clock_t now_time;
        float delta_time;
        double total_time;

        void init();

        void tick();

        double delta() const;

        double total() const;
    };

    namespace time {
        void init();

        void tick();

        double delta();

        double total();
    }
} // ccc
