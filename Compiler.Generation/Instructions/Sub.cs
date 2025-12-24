namespace Compiler.Generation.Instructions;

public sealed record Sub : IBinaryOperator
{
    public static Sub Operator { get; } = new();
    private Sub() { }
    public AssemblyTag Tag => AssemblyTag.Sub;
}