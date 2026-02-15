namespace Compiler.Analysis.Attributes;

public sealed record NoInitializer : StaticInitValue
{
    public static NoInitializer Instance { get; } = new();
    private NoInitializer()
    { }
}