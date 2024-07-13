#pragma once
#include <winrt/windows.foundation.h>
#include <winrt/windows.foundation.collections.h>
#include <winrt/windows.system.threading.h>
#include <parallel_hashmap/phmap.h>
#include "spdlog/spdlog.h"

#include "./utils/utils.h"

#include <iostream>

namespace winrt_col = winrt::Windows::Foundation::Collections;
namespace winrt_thread = winrt::Windows::System::Threading;

#define RT_IID_PPV_ARGS(comtptr) __uuidof(comtptr), comtptr.put_void()

template<class T>
using com_ptr = winrt::com_ptr<T>;
