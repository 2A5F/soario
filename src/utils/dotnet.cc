#include "dotnet.h"

#include <filesystem>

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>

#include "../Args.h"
#include "../render/FrameContext.h"

namespace ccc {
    namespace {
        hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
        hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
        hostfxr_get_runtime_delegate_fn get_delegate_fptr;
        hostfxr_run_app_fn run_app_fptr;
        hostfxr_close_fn close_fptr;

        bool load_hostfxr(const std::filesystem::path &assembly_path);

        load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *assembly);

        void *load_library(const char_t *);

        void *get_export(void *, const char *);
    }

    void load_dotnet() {
        const auto &args = Args::get();
        auto path = std::filesystem::path(args.exe_path);
        path = path.parent_path();
        path.append("managed");

        auto dll_path = std::filesystem::path(path);
        dll_path.append("Soario.dll");
        auto config_path = std::filesystem::path(path);
        config_path.append("Soario.runtimeconfig.json");

        spdlog::debug(std::format("dll path: {}", dll_path.string()));
        spdlog::debug(std::format("dll runtimeconfig path: {}", config_path.string()));

        if (!load_hostfxr(path)) {
            throw std::exception("Failed to load dotnet runtime");
        }

        load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer = nullptr;
        load_assembly_and_get_function_pointer = get_dotnet_load_assembly(config_path.c_str());
        assert(load_assembly_and_get_function_pointer != nullptr && "Failure: get_dotnet_load_assembly()");

        const char_t *dotnet_type = L"Soario.App, Soario";
        const char_t *dotnet_type_method = L"Add";

        typedef int32_t (CORECLR_DELEGATE_CALLTYPE *custom_entry_point_fn)(int32_t a, int32_t b);
        custom_entry_point_fn add = nullptr;
        int rc = load_assembly_and_get_function_pointer(
            dll_path.c_str(),
            dotnet_type,
            dotnet_type_method,
            UNMANAGEDCALLERSONLY_METHOD /*delegate_type_name*/,
            nullptr,
            (void **) &add);

        spdlog::info(std::format("{}", rc));

        if (rc != 0 || add == nullptr) {
            throw std::exception("Failed to load dotnet runtime2");
        }

        auto r = add(123, 456);
        spdlog::info(std::format("{}", r));
    }

    namespace {
        bool load_hostfxr(const std::filesystem::path &assembly_path) {
            get_hostfxr_parameters params{sizeof(get_hostfxr_parameters), assembly_path.c_str(), nullptr};

            // Pre-allocate a large buffer for the path to hostfxr
            char_t buffer[MAX_PATH];
            size_t buffer_size = sizeof(buffer) / sizeof(char_t);
            int rc = get_hostfxr_path(buffer, &buffer_size, &params);
            if (rc != 0)
                return false;

            // Load hostfxr and get desired exports
            // NOTE: The .NET Runtime does not support unloading any of its native libraries. Running
            // dlclose/FreeLibrary on any .NET libraries produces undefined behavior.
            void *lib = load_library(buffer);
            init_for_cmd_line_fptr = (hostfxr_initialize_for_dotnet_command_line_fn) get_export(
                lib, "hostfxr_initialize_for_dotnet_command_line");
            init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn) get_export(
                lib, "hostfxr_initialize_for_runtime_config");
            get_delegate_fptr = (hostfxr_get_runtime_delegate_fn) get_export(lib, "hostfxr_get_runtime_delegate");
            run_app_fptr = (hostfxr_run_app_fn) get_export(lib, "hostfxr_run_app");
            close_fptr = (hostfxr_close_fn) get_export(lib, "hostfxr_close");

            return (init_for_config_fptr && get_delegate_fptr && close_fptr);
        }

        void *load_library(const char_t *path) {
            HMODULE h = ::LoadLibraryW(path);
            assert(h != nullptr);
            return (void *) h;
        }

        void *get_export(void *h, const char *name) {
            void *f = ::GetProcAddress((HMODULE) h, name);
            assert(f != nullptr);
            return f;
        }

        load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *config_path) {
            // Load .NET Core
            void *load_assembly_and_get_function_pointer = nullptr;
            hostfxr_handle cxt = nullptr;
            int rc = init_for_config_fptr(config_path, nullptr, &cxt);
            if (rc != 0 || cxt == nullptr) {
                std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
                close_fptr(cxt);
                return nullptr;
            }

            // Get the load assembly function pointer
            rc = get_delegate_fptr(
                cxt,
                hdt_load_assembly_and_get_function_pointer,
                &load_assembly_and_get_function_pointer);
            if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
                std::cerr << "Get delegate failed: " << std::hex << std::showbase << rc << std::endl;

            close_fptr(cxt);
            return (load_assembly_and_get_function_pointer_fn) load_assembly_and_get_function_pointer;
        }
    }
}
