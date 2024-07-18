#include "Time.h"

#include <chrono>
#include <ctime>

namespace ccc {
    namespace {
        Time s_time;
    }

    void Time::init() {
        data.start_time = data.now_time = data.last_time = std::chrono::steady_clock::now().time_since_epoch().count();
        data.delta_time = data.total_time = 0;
    }

    void Time::tick() {
        data.last_time = data.now_time;
        data.now_time = std::chrono::steady_clock::now().time_since_epoch().count();
        data.delta_time_raw = data.now_time - data.last_time;
        data.total_time_raw = data.now_time - data.start_time;
        data.delta_time = std::chrono::duration<double>(time::delta_raw()).count();
        data.total_time = std::chrono::duration<double>(time::total_raw()).count();
    }

    double Time::delta() const {
        return data.delta_time;
    }

    double Time::total() const {
        return data.total_time;
    }

    TimeData *time::get_data_ptr() {
        return &s_time.data;
    }

    void time::init() {
        s_time.init();
    }

    void time::tick() {
        s_time.tick();
    }

    std::chrono::steady_clock::duration time::delta_raw() {
        return std::chrono::steady_clock::duration(s_time.data.delta_time_raw);
    }

    std::chrono::steady_clock::duration time::total_raw() {
        return std::chrono::steady_clock::duration(s_time.data.total_time_raw);
    }

    double time::delta() {
        return s_time.delta();
    }

    double time::total() {
        return s_time.total();
    }
} // ccc
