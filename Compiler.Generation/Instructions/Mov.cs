namespace Compiler.Generation.Instructions;

public sealed record Mov(IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Mov;   
}