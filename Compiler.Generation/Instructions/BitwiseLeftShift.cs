namespace Compiler.Generation.Instructions;

public sealed record BitwiseLeftShift : IBitwiseOperator
{
    public static BitwiseLeftShift Operator { get; } = new();
    private BitwiseLeftShift() { }
    public AssemblyTag Tag => AssemblyTag.LeftShift;
}