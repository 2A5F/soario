#pragma once
#include <exception>
#include <format>
#include <stdexcept>

#include <SDL3/SDL.h>

namespace ccc
{
    struct sdl_error final : std::runtime_error
    {
        explicit sdl_error(char const* const msg)
            : std::runtime_error(std::format("SDL Error: {}", msg))
        {
        }

        explicit sdl_error() : sdl_error(SDL_GetError())
        {
        }
    };
} // ccc
