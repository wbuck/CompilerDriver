namespace Compiler.Generation.Instructions;

public sealed record Pseudo(string Identifier): IOperand
{
    public AssemblyTag Tag => AssemblyTag.Pseudo;
}