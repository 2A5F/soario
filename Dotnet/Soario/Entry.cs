using System.Buffers;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Soario.Native;
using Soario.Utilities;
using Soario.Windowing;

namespace Soario;

public class Entry
{
    [UnmanagedCallersOnly]
    private static unsafe void Init(InitParams* init_params, InitResult* init_result)
    {
        InitLogger();

        AppDomain.CurrentDomain.UnhandledException += static (_, e) =>
        {
            if (e.ExceptionObject is Exception exception)
            {
                Log.Error(exception, "");
            }
            else
            {
                Log.Error("{Error}", e.ExceptionObject);
            }
        };

        AppVars.s_app_vars = init_params->p_vas;
        init_result->fn_vtb = new AppFnVtb
        {
            utf16_get_utf8_max_len = &GetUtf8MaxLength,
            utf16_to_utf8 = &Utf16ToUtf8,
            utf16_to_string8 = &Utf16ToUtf8String,

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
                File.Move("./logs/latest.log", $"./logs/{time_name}_{count}.log");
            }
            catch (Exception e)
            {
                Log.Error(e, "");
            }
        }
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Async(c => c.File("./logs/latest.log"))
            .CreateLogger();
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_minimumLevel")]
    private static extern ref LogEventLevel GetMinimumLevel(Logger logger);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Exit()
    {
        Log.CloseAndFlush();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Start()
    {
        var version = new Option<bool>(["--version", "-v"], " Show version information");
        var debug = new Option<bool>(["--debug", "-D"], "Enable debug mode");
        var root = new RootCommand("Soario a game of automation")
        {
            debug,
            version,
        };
        root.SetHandler(ctx =>
        {
            if (ctx.ParseResult.FindResultFor(version) is { })
            {
                Console.WriteLine(Utils.GetAsmVer(typeof(Entry).Assembly));
                return;
            }
            if (ctx.ParseResult.FindResultFor(debug) is { })
            {
                AppVars.Debug = true;
                GetMinimumLevel((Logger)Log.Logger) = LogEventLevel.Debug;
                Log.Warning("Debug mode enabled");
            }
            App.Start();
        });
        var parser = new CommandLineBuilder(root)
            .UseHelp()
            .UseEnvironmentVariableDirective()
            .UseParseDirective()
            .UseSuggestDirective()
            .RegisterWithDotnetSuggest()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            .UseExceptionHandler()
            .CancelOnProcessTermination()
            .Build();
        parser.Invoke(Environment.GetCommandLineArgs().Skip(1).ToArray());
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
    private static unsafe FString8* Utf16ToUtf8String(FrStr16 str16)
    {
        var span = str16.AsSpan();
        var array = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(span.Length));
        try
        {
            var len = Encoding.UTF8.GetBytes(span, array);
            fixed (byte* ptr = array)
            {
                return FString8.Create(new() { ptr = ptr, len = (nuint)len });
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
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
