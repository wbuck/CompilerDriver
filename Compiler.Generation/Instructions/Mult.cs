namespace Compiler.Generation.Instructions;

public sealed record Mult : IBinaryOperator
{
    public static Mult Operator { get; } = new();
    private Mult() { }
    public AssemblyTag Tag => AssemblyTag.Mult;
}