namespace Compiler.Generation.Instructions;

public sealed record BitwiseAnd : IBitwiseOperator
{
    public static BitwiseAnd Operator { get; } = new();
    private BitwiseAnd() { }
    public AssemblyTag Tag => AssemblyTag.And;
}