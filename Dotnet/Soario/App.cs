using Serilog;
using Soario.Rendering;
using Soario.Utils;
using Soario.Windowing;

namespace Soario;

public static class App
{
    public static Window MainWindow = null!;
    public static void Start()
    {
        Shaders.LoadShaders().Wait();

        Log.Information("Start");
        MainWindow = new Window(new() { Title = "测试", Size = new(1280, 720), MinSize = new(640, 360) });

        Log.Information("{Device}", Gpu.Instance.MainDevice);
        Log.Information("{Queue}", Gpu.Instance.MainDevice.CommonQueue);
        Log.Information("{Queue}", Gpu.Instance.MainDevice.ComputeQueue);
        Log.Information("{Queue}", Gpu.Instance.MainDevice.CopyQueue);

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}
