namespace Compiler.Generation.Instructions;

public sealed record BitwiseOr : IBitwiseOperator
{
    public static BitwiseOr Operator { get; } = new();
    private BitwiseOr() { }
    public AssemblyTag Tag => AssemblyTag.Or;
}