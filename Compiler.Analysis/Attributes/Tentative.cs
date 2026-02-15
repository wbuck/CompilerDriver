namespace Compiler.Analysis.Attributes;

public sealed record Tentative : StaticInitValue
{
    public static Tentative Instance { get; } = new();
    private Tentative()
    { }
}