cmake_minimum_required(VERSION 3.28)
project(soario)

include(FetchContent)

set(CMAKE_CXX_STANDARD 23)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

set(VCPKG_LIBRARY_LINKAGE static)

LIST(APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_LIST_DIR}/cmake")

if (CMAKE_BUILD_TYPE MATCHES "Release")
    message(dotnet_use_static)
    add_definitions(-DNETHOST_USE_AS_STATIC)
endif ()

add_definitions(-DUNICODE)
add_definitions(-D_UNICODE)
set(SPDLOG_FMT_EXTERNAL ON)
set(SDL_SHARED OFF)
set(SDL_STATIC ON)
set(SDL_VULKAN OFF)
set(SDL_CAMERA OFF)
set(SDL_RENDER OFF)
set(SDL_HAPTIC OFF)
set(SDL_SENSOR OFF)
set(SDL_POWER OFF)
set(SDL_HIDAPI OFF)
set(SDL_DIALOG OFF)

add_subdirectory(third_party/SDL EXCLUDE_FROM_ALL)
add_subdirectory(third_party/mimalloc EXCLUDE_FROM_ALL)
find_package(Dotnet 8.0 REQUIRED)
find_package(cppwinrt CONFIG REQUIRED)
find_package(directx-headers CONFIG REQUIRED)
find_package(directx12-agility CONFIG REQUIRED)
find_package(directxmath CONFIG REQUIRED)
find_package(directxtk12 CONFIG REQUIRED)
find_package(directx-dxc CONFIG REQUIRED)
find_path(PARALLEL_HASHMAP_INCLUDE_DIRS "parallel_hashmap/btree.h")
find_package(stduuid CONFIG REQUIRED)
find_package(args CONFIG REQUIRED)
find_package(spdlog CONFIG REQUIRED)
find_package(fmt CONFIG REQUIRED)

add_custom_target(shaders)
add_custom_target(dotnet)

set(HLSL_ITEMS src/shader/line.meta)
set(HLSL_ITEMS src/shader/rect.meta)

add_executable(${PROJECT_NAME} WIN32 src/main.cpp
        src/pch.h
        src/utils/utils.cc
        src/utils/utils.h
        src/utils/set_thread_name.cc
        src/utils/console_utils.cc
        src/window/Window.cc
        src/window/Window.h
        src/utils/sdl_error.cc
        src/utils/sdl_error.h
        third_party/D3D12MemoryAllocator/include/D3D12MemAlloc.h
        third_party/D3D12MemoryAllocator/src/D3D12MemAlloc.cpp
        src/render/GpuQueue.cc
        src/render/GpuQueue.h
        src/render/FrameContext.cc
        src/render/FrameContext.h
        src/render/GpuCommandList.cc
        src/render/GpuCommandList.h
        src/App.cc
        src/App.h
        src/render/Shader.cc
        src/render/Shader.h
        src/utils/new-delete.cc
        src/utils/dotnet.cc
        src/utils/dotnet.h
        src/ffi/Init.h
        src/ffi/App.h
        src/ffi/FnPtrs.h
        src/ffi/FWindow.cc
        src/ffi/FWindow.h
        src/ffi/FFI.h
        src/utils/utf8_utf16.cc
        src/utils/utf8_utf16.h
        src/utils/String.cc
        src/utils/String.h
        src/utils/Rc.h
        src/ffi/FFI.cc
        src/render/Gpu.cc
        src/render/Gpu.h
        src/ffi/gpu/FGpu.h
        src/ffi/gpu/FGpu.cc
        src/utils/Err.h
        src/ffi/gpu/FShader.cc
        src/ffi/gpu/FShader.h
        src/ffi/FMemory.cc
        src/ffi/FMemory.h
        src/ffi/gpu/FGpuQueue.cc
        src/ffi/gpu/FGpuQueue.h
        src/ffi/gpu/FGpuDevice.cc
        src/ffi/gpu/FGpuDevice.h
        src/render/GpuDevice.cc
        src/render/GpuDevice.h
        src/ffi/FLogLevel.h
        src/utils/logger.h
        src/ffi/gpu/FGpuSurface.cc
        src/ffi/gpu/FGpuSurface.h
        src/render/GpuSurfaceHwnd.cc
        src/render/GpuSurfaceHwnd.h
        src/render/GpuSurface.cc
        src/render/GpuSurface.h
        src/ffi/gpu/FGpuRt.cc
        src/ffi/gpu/FGpuRt.h
        src/ffi/FString8.cc
        src/ffi/FString8.h
        src/ffi/gpu/FGpuCmd.cc
        src/ffi/gpu/FGpuCmd.h
        src/ffi/gpu/FGpuRes.cc
        src/ffi/gpu/FGpuRes.h
        src/render/State.cc
        src/render/State.h
        src/ffi/gpu/FGpuPipelineLayout.cc
        src/ffi/gpu/FGpuPipelineLayout.h
        src/render/GpuBindLessPipelineLayout.cc
        src/render/GpuBindLessPipelineLayout.h
        src/render/GpuPipelineLayout.cc
        src/render/GpuPipelineLayout.h
        src/ffi/gpu/FGpuPipelineState.cc
        src/ffi/gpu/FGpuPipelineState.h
        src/render/GpuPipelineState.cc
        src/render/GpuPipelineState.h
        src/render/GpuFencePak.cc
        src/render/GpuFencePak.h
        src/ffi/gpu/FGpuFormat.cc
        src/ffi/gpu/FGpuFormat.h
        src/render/GpuPipelineConvert.cc
        src/render/GpuPipelineConvert.h
)

add_dependencies(${PROJECT_NAME} shaders)
add_dependencies(${PROJECT_NAME} dotnet)

target_include_directories(${PROJECT_NAME} PRIVATE third_party/D3D12MemoryAllocator/include)
target_include_directories(${PROJECT_NAME} PRIVATE third_party/microsoft.netcore.app.host.win-x64/runtimes/win-x64/native)
target_link_directories(${PROJECT_NAME} PRIVATE third_party/microsoft.netcore.app.host.win-x64/runtimes/win-x64/native)

target_link_libraries(${PROJECT_NAME} PRIVATE SDL3::SDL3)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::CppWinRT RuntimeObject dwmapi dxgi.lib)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectX-Headers Microsoft::DirectX-Guids Microsoft::DirectX12-Agility)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXMath)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXTK12)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXShaderCompiler)
target_include_directories(${PROJECT_NAME} PRIVATE ${PARALLEL_HASHMAP_INCLUDE_DIRS})
target_link_libraries(${PROJECT_NAME} PRIVATE stduuid)
target_link_libraries(${PROJECT_NAME} PRIVATE taywee::args)
target_link_libraries(${PROJECT_NAME} PRIVATE fmt::fmt-header-only)
target_link_libraries(${PROJECT_NAME} PRIVATE spdlog::spdlog_header_only)
target_link_libraries(${PROJECT_NAME} PRIVATE mimalloc-static)

target_link_libraries(${PROJECT_NAME} PRIVATE nethost.lib)
configure_file(${CMAKE_CURRENT_SOURCE_DIR}/third_party/microsoft.netcore.app.host.win-x64/runtimes/win-x64/native/nethost.dll ${CMAKE_BINARY_DIR}/bin/nethost.dll COPYONLY)

if (TARGET Microsoft::DirectX12-Agility)
    file(MAKE_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/D3D12")
    add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_PROPERTY:Microsoft::DirectX12-Core,IMPORTED_LOCATION_RELEASE> $<TARGET_FILE_DIR:${PROJECT_NAME}>/D3D12
            COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_PROPERTY:Microsoft::DirectX12-Layers,IMPORTED_LOCATION_DEBUG> $<TARGET_FILE_DIR:${PROJECT_NAME}>/D3D12
            COMMAND_EXPAND_LISTS
    )
endif ()

foreach (FILE ${HLSL_ITEMS})
    get_filename_component(FILE_WE ${FILE} NAME_WE)
    add_custom_command(TARGET shaders
            POST_BUILD
            COMMAND deno run -A ./scripts/build_shader.ts ${DIRECTX_DXC_TOOL} ${CMAKE_BINARY_DIR} ${FILE_WE} ${FILE} $<IF:$<CONFIG:DEBUG>,D,R> ${CMAKE_CURRENT_SOURCE_DIR}/src/ ${CMAKE_CURRENT_SOURCE_DIR}/tools/Coplt.ShaderReflections.Dxc/Coplt.ShaderReflections.Dxc.exe
            MAIN_DEPENDENCY ${FILE}
            WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
            VERBATIM)
endforeach (FILE)

add_custom_command(TARGET dotnet
        POST_BUILD
        COMMAND deno run -A ./scripts/build_dotnet.ts ${DOTNET_EXE}
        ${CMAKE_BINARY_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/Dotnet/Soario/Soario.csproj $<IF:$<CONFIG:DEBUG>,D,R>
        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
        VERBATIM
)
