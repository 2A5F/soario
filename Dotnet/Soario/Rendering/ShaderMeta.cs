namespace Soario.Rendering;

public enum ShaderStage
{
    Ps = 1,
    Vs,
    Cs,
    Ms,
    As,
}

public record struct ShaderMeta
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public Dictionary<string, ShaderPassMeta> Pass { get; set; }
}

public record struct ShaderPassMeta
{
    public List<string> Stages { get; set; }
}
