using System.Runtime.CompilerServices;

namespace Soario;

public static unsafe class AppVars
{
    internal static Soario.Native.AppVars* s_app_vars;

    public static ref bool Debug
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Unsafe.As<byte, bool>(ref s_app_vars->debug);
    }
}
