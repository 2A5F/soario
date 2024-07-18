#pragma once
#include <optional>
#include <string>

#include "Api.h"

namespace ccc {
    struct WindowCreateOptions {
        FrStr16 title;
        FInt2 size;
        FInt2 min_size;
        FBool has_min_size;
    };

    struct FWindow : virtual FObject {
        __declspec(dllexport) static FWindow *create(const WindowCreateOptions &options);
    };
} // ccc
