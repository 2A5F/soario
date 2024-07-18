#include "String.h"

namespace ccc {
    String::String() : m_ptr(nullptr), m_len(0) {
    }

    String::String(const uint8_t *ptr, size_t len) : m_ptr(ptr), m_len(len) {
    }

    String::String(String &&other) noexcept : m_ptr(other.m_ptr), m_len(other.len()) {
        other.m_ptr = nullptr;
        other.m_len = 0;
    }

    String &String::operator=(String &&other) noexcept {
        m_ptr = other.m_ptr;
        m_len = other.m_len;
        other.m_ptr = nullptr;
        other.m_len = 0;
        return *this;
    }

    size_t String::len() const {
        return m_len;
    }

    const uint8_t *String::ptr() const {
        return m_ptr;
    }

    const char *String::c_str() const {
        return reinterpret_cast<const char *>(m_ptr);
    }

    std::string String::to_std_string() const {
        return std::string(c_str(), len());
    }

    String::~String() {
        if (m_ptr != nullptr) delete[] m_ptr;
    }
} // ccc
