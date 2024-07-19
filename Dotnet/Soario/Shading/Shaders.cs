using Soario.Resources;

namespace Soario.Shading;

public static class Shaders
{
    public static Task LoadShaders()
    {
        var dir = Path.Join(Environment.ProcessPath, "..", "assets", "shaders");
        var files = GetAllFiles(dir);
        return Task.WhenAll(
            files.Where(static path => Path.GetExtension(path) == ".shader")
                .Select(Shader.Load)
                .Select(async t =>
                {
                    var shader = await t;
                    Assets.RegisterAsset(shader);
                })
        );
    }

    private static IEnumerable<string> GetAllFiles(string dir)
    {
        foreach (var path in Directory.EnumerateFileSystemEntries(dir))
        {
            var attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) != 0)
            {
                foreach (var file in GetAllFiles(path))
                {
                    yield return file;
                }
            }
            else
            {
                yield return path;
            }
        }
    }
}
