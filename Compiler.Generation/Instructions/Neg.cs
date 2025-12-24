namespace Compiler.Generation.Instructions;

public sealed record Neg : IUnaryOperator
{
    public static Neg Operator { get; } = new();
    private Neg() { }
    public AssemblyTag Tag => AssemblyTag.Neg;
}