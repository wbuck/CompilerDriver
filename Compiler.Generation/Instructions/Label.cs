namespace Compiler.Generation.Instructions;

public sealed record Label(string Identifier) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Label;
}