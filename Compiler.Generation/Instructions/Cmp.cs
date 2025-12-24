namespace Compiler.Generation.Instructions;

public sealed record Cmp(IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Cmp;
}