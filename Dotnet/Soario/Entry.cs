using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;
using Serilog.Events;
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
        InitLogger();

        Time.p_time_data = init_params->p_time_data;
        Gpu.s_gpu = new(init_params->p_gpu);
        init_result->fn_vtb = new AppFnVtb
        {
            utf16_get_utf8_max_len = &GetUtf8MaxLength,
            utf16_to_utf8 = &Utf16ToUtf8,

            start = &Start,
            exit = &Exit,

            window_event_handle = &Window.EventHandle,

            logger_cstr = &Logger,
            logger_wstr = &Logger,
            logger_str8 = &Logger,
            logger_str16 = &Logger,
        };
        Log.Debug("Init Dotnet");
    }

    private static void InitLogger()
    {
        if (File.Exists("./logs/latest.log"))
        {
            try
            {
                var time = File.GetCreationTime("./logs/latest.log");
                var time_name = $"{time:yyyy-MM-dd}";
                var max_count = Directory.GetFiles("./logs/")
                    .Where(static n => Path.GetExtension(n) == ".log")
                    .Select(static n => Path.GetFileName(n))
                    .Where(n => n.StartsWith(time_name))
                    .Select(n => n.Substring(time_name.Length))
                    .Select(static n => (n, i: n.IndexOf('.')))
                    .Where(static a => a.i > 1)
                    .Select(static a => (s: uint.TryParse(a.n.Substring(1, a.i - 1), out var n), n))
                    .Where(static a => a.s)
                    .OrderByDescending(static a => a.n)
                    .Select(static a => a.n)
                    .FirstOrDefault();
                var count = max_count + 1;
                File.Move("./logs/latest.log", $"./logs/{time_name}-{count}.log");
            }
            catch (Exception e)
            {
                Log.Error(e, "");
            }
        }
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Async(c => c.File("./logs/latest.log"))
            .CreateLogger();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Exit()
    {
        Log.Information("exited");
        Log.CloseAndFlush();
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

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Logger(FLogLevel level, sbyte* msg)
    {
        var str = Marshal.PtrToStringUTF8((IntPtr)msg);
        Log.Write(level.ToLogEventLevel(), "[Native] {Message}", str);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void Logger(FLogLevel level, ushort* msg)
    {
        var str = Marshal.PtrToStringUni((IntPtr)msg);
        Log.Write(level.ToLogEventLevel(), "[Native] {Message}", str);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Logger(FLogLevel level, FrStr8 msg)
    {
        var str = msg.ToString();
        Log.Write(level.ToLogEventLevel(), "[Native] {Message}", str);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Logger(FLogLevel level, FrStr16 msg)
    {
        var str = msg.ToString();
        Log.Write(level.ToLogEventLevel(), "[Native] {Message}", str);
    }
}
