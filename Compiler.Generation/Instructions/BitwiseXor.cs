namespace Compiler.Generation.Instructions;

public sealed record BitwiseXor : IBitwiseOperator
{
    public static BitwiseXor Operator { get; } = new();
    private BitwiseXor() { }
    public AssemblyTag Tag => AssemblyTag.Xor;
}