using Soario.Native;

namespace Soario;

public static class Application
{
    public static void Exit(int code = 0)
    {
        FFI.exit(code);
    }
}
