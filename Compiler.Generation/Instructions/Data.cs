namespace Compiler.Generation.Instructions;

public sealed record Data(string Identifier) : IOperand
{
    public AssemblyTag Tag => AssemblyTag.Data;
}