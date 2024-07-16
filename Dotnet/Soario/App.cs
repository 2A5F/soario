using System.Runtime.InteropServices;

namespace Soario;

public class App
{
    [UnmanagedCallersOnly]
    public static int Add(int a, int b) => a + b;
}
