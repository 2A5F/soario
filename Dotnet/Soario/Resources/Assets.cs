using System.Collections.Concurrent;

namespace Soario.Resources;

public static class Assets
{
    private static readonly ConcurrentDictionary<Guid, AAsset> s_id_shaders = new();
    private static readonly ConcurrentDictionary<string, AAsset> s_path_shaders = new();

    public static T? TryGet<T>(string path) where T : AAsset =>
        s_path_shaders.TryGetValue(path, out var asset) ? asset as T : null;
    
    public static T? TryGet<T>(Guid id) where T : AAsset =>
        s_id_shaders.TryGetValue(id, out var asset) ? asset as T : null;

    internal static void RegisterAsset(AAsset asset)
    {
        s_id_shaders[asset.Id] = asset;
        if (asset.Path is { } path)
        {
            s_path_shaders[path] = asset;
        }
    }
}
