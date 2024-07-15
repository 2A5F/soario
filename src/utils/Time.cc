#include "Time.h"

#include <chrono>
#include <ctime>

namespace ccc {
    namespace {
        Time s_time;
    }

    void Time::init() {
        start_time = now_time = last_time = std::chrono::steady_clock::now();
        delta_time = total_time = 0;
    }

    void Time::tick() {
        last_time = now_time;
        now_time = std::chrono::steady_clock::now();
        delta_time_raw = std::chrono::duration(now_time - last_time);
        total_time_raw = std::chrono::duration(now_time - start_time);
        delta_time = std::chrono::duration<double>(delta_time_raw).count();
        total_time = std::chrono::duration<double>(total_time_raw).count();
    }

    double Time::delta() const {
        return delta_time;
    }

    double Time::total() const {
        return total_time;
    }

    void time::init() {
        s_time.init();
    }

    void time::tick() {
        s_time.tick();
    }

    std::chrono::steady_clock::duration time::delta_raw() {
        return s_time.delta_time_raw;
    }

    std::chrono::steady_clock::duration time::total_raw() {
        return s_time.total_time_raw;
    }

    double time::delta() {
        return s_time.delta();
    }

    double time::total() {
        return s_time.total();
    }
} // ccc
