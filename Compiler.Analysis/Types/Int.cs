namespace Compiler.Analysis.Types;

public readonly record struct Int : IType
{
    public static Int Instance { get; } = new();
    public string TypeName => "int";
}