#include "FString8.h"

#include "../utils/String.h"

namespace ccc
{
    FString8* FString8::Create(const FrStr8 slice) noexcept
    {
        auto r = String::CreateCopy(slice);
        return r.leak();
    }
} // ccc
