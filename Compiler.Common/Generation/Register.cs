namespace Compiler.Common.Generation;

public record Register : IOperand
{
    public OperationType Type 
        => OperationType.Register;

    public string Build() => "%eax";
}