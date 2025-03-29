namespace Compiler.Common.Generation;

public record Imm<T> : IOperand
{
    private readonly string _value;

    public Imm(T value)
    {
        _value = $"${value}";
    }
    
    public OperationType Type 
        => OperationType.Immediate;

    public string Build() => _value;
}