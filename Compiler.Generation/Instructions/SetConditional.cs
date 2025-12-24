namespace Compiler.Generation.Instructions;

public sealed record SetConditional(IConditionCode Code, IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.SetConditional;
}