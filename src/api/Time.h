#pragma once
#include <cstdint>

namespace ccc {
    struct TimeData {
        int64_t start_time;
        int64_t last_time;
        int64_t now_time;
        int64_t delta_time_raw;
        int64_t total_time_raw;
        double delta_time;
        double total_time;
    };
}
