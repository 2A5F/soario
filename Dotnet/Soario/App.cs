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

        Task.Factory.StartNew(() =>
        {
            var device = Gpu.Instance.MainDevice;
            var surface = device.CreateSurface(MainWindow, new GpuSurfaceCreateOptions { Name = "MainSurface" });

            Log.Information("{Device}", Gpu.Instance.MainDevice);
            Log.Information("{Queue}", Gpu.Instance.MainDevice.CommonQueue);
            Log.Information("{Queue}", Gpu.Instance.MainDevice.ComputeQueue);
            Log.Information("{Queue}", Gpu.Instance.MainDevice.CopyQueue);
            Log.Information("{Surface}", surface);
        }, TaskCreationOptions.LongRunning);

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}
