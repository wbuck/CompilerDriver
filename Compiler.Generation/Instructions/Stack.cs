namespace Compiler.Generation.Instructions;

public sealed record Stack(int Offset) : IOperand
{
    public AssemblyTag Tag => AssemblyTag.Stack;
}