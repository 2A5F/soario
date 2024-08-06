#pragma once
#include "../App.h"
#include "../ffi/FLogLevel.h"

namespace ccc
{
    namespace logger
    {
        template <const size_t N>
        void write(const FLogLevel level, const char (&msg)[N])
        {
            app_fn_vtb().logger_str8(level, {reinterpret_cast<const uint8_t*>(&*msg), sizeof(msg) - 1});
        }

        template <const size_t N>
        void write(const FLogLevel level, const char16_t (&msg)[N])
        {
            app_fn_vtb().logger_str16(level, {reinterpret_cast<const uint16_t*>(&*msg), sizeof(msg) - 1});
        }

        inline void write(const FLogLevel level, const char* msg)
        {
            app_fn_vtb().logger_cstr(level, msg);
        }

        inline void write(const FLogLevel level, const wchar_t* msg)
        {
            app_fn_vtb().logger_wstr(level, msg);
        }

        inline void write(const FLogLevel level, const std::string& msg)
        {
            app_fn_vtb().logger_str8(level, {reinterpret_cast<const uint8_t*>(msg.data()), msg.size()});
        }

        inline void write(const FLogLevel level, const std::wstring& msg)
        {
            app_fn_vtb().logger_str16(level, {reinterpret_cast<const uint16_t*>(msg.data()), msg.size()});
        }

        inline void write(const FLogLevel level, const winrt::hstring& msg)
        {
            app_fn_vtb().logger_str16(level, {reinterpret_cast<const uint16_t*>(msg.data()), msg.size()});
        }

        template <typename T, const size_t N>
        void info(T (&msg)[N])
        {
            return write<N>(FLogLevel::Info, msg);
        }

        template <typename T>
        void info(T msg)
        {
            return write(FLogLevel::Info, msg);
        }

        template <typename T, const size_t N>
        void error(T (&msg)[N])
        {
            return write<N>(FLogLevel::Error, msg);
        }

        template <typename T>
        void error(T msg)
        {
            return write(FLogLevel::Error, msg);
        }

        template <typename T, const size_t N>
        void warn(T (&msg)[N])
        {
            return write<N>(FLogLevel::Warn, msg);
        }

        template <typename T>
        void warn(T msg)
        {
            return write(FLogLevel::Warn, msg);
        }

        template <typename T, const size_t N>
        void debug(T (&msg)[N])
        {
            return write<N>(FLogLevel::Debug, msg);
        }

        template <typename T>
        void debug(T msg)
        {
            return write(FLogLevel::Debug, msg);
        }
    }
}
