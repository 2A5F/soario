namespace Soario.Shading;

public enum ShaderStage
{
    Ps = 1,
    Vs,
    Gs,
    Hs,
    Ds,
    Cs,
    Lin,
    Ms,
    As,
}

public record struct ShaderMeta
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public Dictionary<string, ShaderPassMeta> Items { get; set; } 
}

public record struct ShaderPassMeta
{
    public ShaderStage Type { get; set; }
}
