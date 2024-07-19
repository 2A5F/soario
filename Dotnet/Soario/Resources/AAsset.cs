namespace Soario.Resources;

public abstract class AAsset(Guid id, string? path)
{
    public Guid Id { get; } = id;
    public string? Path { get; } = path;
}
