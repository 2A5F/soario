#include "Time.h"

#include <ctime>

namespace ccc {
    namespace {
        Time s_time;
    }

    void Time::init() {
        start_time = now_time = last_time = clock();
        delta_time = total_time = 0;
    }

    void Time::tick() {
        last_time = now_time;
        now_time = clock();
        delta_time = static_cast<double>(now_time - last_time) / CLOCKS_PER_SEC;
        total_time = static_cast<double>(now_time - start_time) / CLOCKS_PER_SEC;
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

    double time::delta() {
        return s_time.delta();
    }

    double time::total() {
        return s_time.total();
    }
} // ccc
