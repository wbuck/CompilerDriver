namespace Compiler.Generation.Instructions;

public sealed record Binary(IBinaryOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Binary; 
}