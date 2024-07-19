namespace Soario.Shading;

public static class Shaders
{
    private static Dictionary<string, Shader> s_shaders = new();
    public static void LoadShaders()
    {
        var dir = Path.Join(Environment.ProcessPath, "shader");
        LoadShaders(dir);
    }

    private static void LoadShaders(string dir) { }
}
