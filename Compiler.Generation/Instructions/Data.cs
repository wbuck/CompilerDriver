namespace Compiler.Generation.Instructions;

public sealed record Data(string Identifier) : IMemory
{
    public AssemblyTag Tag => AssemblyTag.Data;
}