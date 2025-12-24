namespace Compiler.Generation.Instructions;

public sealed record DeallocateStack(int Offset) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.DeallocateStack;
}