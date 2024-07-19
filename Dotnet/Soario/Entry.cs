using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Soario.Native;
using Soario.Rendering;
using Soario.Utils;
using Soario.Windowing;

namespace Soario;

public class Entry
{
    [UnmanagedCallersOnly]
    private static unsafe void Init(InitParams* init_params, InitResult* init_result)
    {
        Time.p_time_data = init_params->p_time_data;
        Gpu.s_gpu = new(init_params->p_gpu);
        init_result->fn_vtb = new AppFnVtb
        {
            utf16_get_utf8_max_len = &GetUtf8MaxLength,
            utf16_to_utf8 = &Utf16ToUtf8,

            start = &Start,

            window_event_handle = &Window.EventHandle,
        };
        Console.WriteLine("Init Dotnet");
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Start()
    {
        App.Start();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static nuint GetUtf8MaxLength(FrStr16 str16)
    {
        return (nuint)Encoding.UTF8.GetMaxByteCount((int)str16.len);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static nuint Utf16ToUtf8(FrStr16 str16, FmStr8 str8)
    {
        var len = Encoding.UTF8.GetBytes(str16.AsSpan(), str8.AsSpan());
        str8.AsSpan()[len] = 0;
        return (nuint)len;
    }
}
