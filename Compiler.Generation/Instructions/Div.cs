namespace Compiler.Generation.Instructions;

public sealed record Div(IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Div;
}