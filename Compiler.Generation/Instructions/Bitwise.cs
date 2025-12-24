namespace Compiler.Generation.Instructions;

public sealed record Bitwise(IBitwiseOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Bitwise; 
}