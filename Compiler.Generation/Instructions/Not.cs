namespace Compiler.Generation.Instructions;

public sealed record Not : IUnaryOperator
{
    public static Not Operator { get; } = new();
    private Not() { }
    public AssemblyTag Tag => AssemblyTag.Not;
}