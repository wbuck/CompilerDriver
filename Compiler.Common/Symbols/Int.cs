namespace Compiler.Common.Symbols;

public readonly record struct Int : IType
{
    public static Int Instance { get; } = new();
    public string TypeName => "int";
}