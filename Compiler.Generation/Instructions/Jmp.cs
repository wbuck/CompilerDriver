namespace Compiler.Generation.Instructions;

public sealed record Jmp(string Target) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Jmp;
}