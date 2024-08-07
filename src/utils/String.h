#pragma once
#include <cstdint>
#include <string>

#include "../ffi/FFI.h"
#include "Rc.h"
#include "../ffi/FString8.h"

namespace ccc
{
    // 只读的堆分配字符串
    class String final : public FString8
    {
        IMPL_RC(String);

        String();

        explicit String(const uint8_t* ptr, size_t len);

    public:
        static Rc<String> CreateCopy(FrStr8 slice);

        static Rc<String> Create(size_t len);

        template <class F>
        static Rc<String> Create(const size_t cap, F f);

        String(String& other) = delete;

        String(String&& other) = delete;

        String& operator=(String& other) = delete;

        String& operator=(String&& other) = delete;

        size_t len() const;

        const uint8_t* ptr() const;

        const char* c_str() const;

        // 将产生复制
        std::string to_std_string() const;
    };

    template <class F>
    Rc<String> String::Create(const size_t cap, F f)
    {
        const auto size = sizeof(String) + cap + 1;
        const auto mem = static_cast<char*>(operator new(size));
        mem[size - 1] = 0;
        const auto ptr = reinterpret_cast<uint8_t*>(mem + sizeof(String));
        const auto len = f(ptr);
        new(mem) String(ptr, len);
        return Rc(reinterpret_cast<String*>(mem));
    }
} // ccc
