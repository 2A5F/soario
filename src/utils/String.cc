#include "String.h"

namespace ccc
{
    String::String() : FString8(nullptr, 0)
    {
    }

    String::String(const uint8_t* ptr, const size_t len) : FString8(ptr, len)
    {
    }

    Rc<String> String::CreateCopy(FrStr8 slice)
    {
        return Create(
            slice.len, [slice](auto ptr)
            {
                memcpy(ptr, slice.ptr, slice.len);
                return slice.len;
            }
        );
    }

    Rc<String> String::Create(const size_t len)
    {
        const auto size = sizeof(String) + len + 1;
        const auto mem = static_cast<char*>(operator new(size));
        mem[size - 1] = 0;
        const auto ptr = reinterpret_cast<uint8_t*>(mem + sizeof(String));
        new(mem) String(ptr, len);
        return Rc(reinterpret_cast<String*>(mem));
    }

    size_t String::len() const
    {
        return m_len;
    }

    const uint8_t* String::ptr() const
    {
        return m_ptr;
    }

    const char* String::c_str() const
    {
        return reinterpret_cast<const char*>(m_ptr);
    }

    std::string String::to_std_string() const
    {
        return std::string(c_str(), len());
    }
} // ccc
