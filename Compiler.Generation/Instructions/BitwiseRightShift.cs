namespace Compiler.Generation.Instructions;

public sealed record BitwiseRightShift : IBitwiseOperator
{
    public static BitwiseRightShift Operator { get; } = new();
    private BitwiseRightShift() { }
    public AssemblyTag Tag => AssemblyTag.RightShift;
}