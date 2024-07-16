#include "dotnet.h"

namespace ccc {

    namespace {
        hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
        hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
        hostfxr_get_runtime_delegate_fn get_delegate_fptr;
        hostfxr_run_app_fn run_app_fptr;
        hostfxr_close_fn close_fptr;
    }

    void load_dotnet() {

    }
}
