cmake_minimum_required(VERSION 3.28)
project(soario)

set(CMAKE_CXX_STANDARD 23)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

set(SDL_SHARED OFF)
set(SDL_STATIC ON)

find_package(directx-headers CONFIG REQUIRED)
find_package(directx12-agility CONFIG REQUIRED)
find_package(directxmath CONFIG REQUIRED)
find_package(directxtk12 CONFIG REQUIRED)
find_package(directx-dxc CONFIG REQUIRED)
find_package(d3d12-memory-allocator CONFIG REQUIRED)
find_package(thread-pool CONFIG REQUIRED)
find_path(PARALLEL_HASHMAP_INCLUDE_DIRS "parallel_hashmap/btree.h")

add_executable(${PROJECT_NAME} src/main.cpp)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectX-Headers Microsoft::DirectX-Guids Microsoft::DirectX12-Agility)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXMath)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXTK12)
target_link_libraries(${PROJECT_NAME} PRIVATE Microsoft::DirectXShaderCompiler)
target_link_libraries(${PROJECT_NAME} PRIVATE unofficial::D3D12MemoryAllocator)
target_link_libraries(${PROJECT_NAME} PRIVATE dp::thread-pool)
target_include_directories(${PROJECT_NAME} PRIVATE ${PARALLEL_HASHMAP_INCLUDE_DIRS})

if (TARGET Microsoft::DirectX12-Agility)
    file(MAKE_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/D3D12")
    add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_PROPERTY:Microsoft::DirectX12-Core,IMPORTED_LOCATION_RELEASE> $<TARGET_FILE_DIR:${PROJECT_NAME}>/D3D12
            COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_PROPERTY:Microsoft::DirectX12-Layers,IMPORTED_LOCATION_DEBUG> $<TARGET_FILE_DIR:${PROJECT_NAME}>/D3D12
            COMMAND_EXPAND_LISTS
    )
endif ()
