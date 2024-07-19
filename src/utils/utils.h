#pragma once
#include <directxmath.h>
#include <windows.h>
#include <string>

void RedirectIOToConsole();

void SetSetThreadName(DWORD dwThreadID, const char* threadName);

std::wstring Utf8ToUtf16(char const* str);

using int2 = DirectX::XMINT2;
using int3 = DirectX::XMINT3;
using int4 = DirectX::XMINT4;

using float2 = DirectX::XMFLOAT2A;
using float3 = DirectX::XMFLOAT3A;
using float4 = DirectX::XMFLOAT4A;

using float3x3 = DirectX::XMFLOAT3X3;
using float4x4 = DirectX::XMFLOAT4X4A;
