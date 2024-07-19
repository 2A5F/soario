#include "utils.h"
#include "../pch.h"

std::wstring Utf8ToUtf16(char const* str)
{
    const auto len = MultiByteToWideChar(CP_UTF8, MB_PRECOMPOSED, str, -1, nullptr, 0);
    if (len <= 0) return L"";
    std::wstring ws(len, '\0');
    MultiByteToWideChar(CP_UTF8, MB_PRECOMPOSED, str, -1, (LPWSTR)ws.c_str(), len);
    return ws;
}
