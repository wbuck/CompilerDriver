namespace Compiler.Generation.Instructions;

public sealed record Stack(int Offset) : IMemory
{
    public AssemblyTag Tag => AssemblyTag.Stack;
}