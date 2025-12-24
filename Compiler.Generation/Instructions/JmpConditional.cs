namespace Compiler.Generation.Instructions;

public sealed record JmpConditional(IConditionCode Code, string Target) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.JmpConditional;
}