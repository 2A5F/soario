#pragma once
namespace ccc {
    struct Args {
        bool debug;

        static void set(Args args);

        static const Args& get();
    };
} // ccc
