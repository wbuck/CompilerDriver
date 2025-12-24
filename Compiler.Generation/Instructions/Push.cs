namespace Compiler.Generation.Instructions;

public sealed record Push(IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Push;
}