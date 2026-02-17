namespace Compiler.Common.Symbols;

public sealed record NoInitializer : StaticInitValue
{
    public static NoInitializer Instance { get; } = new();
    private NoInitializer()
    { }
}