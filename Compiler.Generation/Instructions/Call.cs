namespace Compiler.Generation.Instructions;

public sealed record Call(string Identifier) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Call;
}