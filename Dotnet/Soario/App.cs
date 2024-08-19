using System.Runtime.ExceptionServices;
using Coplt.Mathematics;
using Serilog;
using Soario.Rendering;
using Soario.Resources;
using Soario.Utilities;
using Soario.Windowing;

namespace Soario;

public static class App
{
    public static Window MainWindow = null!;
    public static void Start()
    {
        Gpu.Init();

        Shaders.LoadShaders().Wait();

        MainWindow = new Window(new() { Title = "测试", Size = new(1280, 720), MinSize = new(640, 360) });

        Task.Factory.StartNew(() =>
        {
            try
            {
                var device = Gpu.Instance.MainDevice;
                var surface = device.CreateSurface(MainWindow, new GpuSurfaceCreateOptions { Name = "Main Surface" });

                Log.Information("{Device}", Gpu.Instance.MainDevice);
                Log.Information("{Queue}", Gpu.Instance.MainDevice.CommonQueue);
                Log.Information("{Queue}", Gpu.Instance.MainDevice.ComputeQueue);
                Log.Information("{Queue}", Gpu.Instance.MainDevice.CopyQueue);
                Log.Information("{Surface}", surface);

                var shader_rect = Assets.TryGet<Shader>("shader/rect")!;
                Log.Information("{Shader}", shader_rect);
                var pipeline = device.CreatePipelineState(device.BindLessPipelineLayout,
                    new(shader_rect.GetPass(0), GpuTextureFormat.R8G8B8A8_UNorm));
                Log.Information("{Pipeline}", pipeline);

                var buffer1 = device.CreateBuffer(new(4) { Name = "Test Buffer" });
                Log.Information("{Buffer}", buffer1);

                var cmd = new GpuCmdList(device);
                while (true)
                {
                    surface.ReadyFrame();

                    cmd.SetRt(surface);
                    cmd.ReadyRasterizer(
                        new RasterizerInfo(new(0, 0, MainWindow.Size), new(0, 1), new(0, 0, MainWindow.Size))
                    );
                    cmd.Clear(new float4(1, 1, 1, 1));
                    cmd.DispatchMesh(pipeline, new(1, 1, 1));

                    surface.PresentFrame(cmd);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "");
            }
        }, TaskCreationOptions.LongRunning);

        while (true)
        {
            Thread.Sleep(100);
        }
    }
}
