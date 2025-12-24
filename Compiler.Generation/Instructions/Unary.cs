namespace Compiler.Generation.Instructions;

public sealed record Unary(IUnaryOperator Operator, IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Unary;  
}