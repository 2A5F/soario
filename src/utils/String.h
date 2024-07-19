#pragma once
#include <cstdint>
#include <string>

#include "Object.h"

namespace ccc
{
    // 只读的堆分配字符串
    class String final : Object
    {
        const uint8_t* m_ptr;
        size_t m_len;

    public:
        String();

        explicit String(/* 将获取所有权，要求 0 结尾 */const uint8_t* ptr, size_t len);

        String(String& other) = delete;

        String(String&& other) noexcept;

        String& operator=(String& other) = delete;

        String& operator=(String&& other) noexcept;

        size_t len() const;

        const uint8_t* ptr() const;

        const char* c_str() const;

        // 将产生复制
        std::string to_std_string() const;

        ~String() override;
    };
} // ccc
