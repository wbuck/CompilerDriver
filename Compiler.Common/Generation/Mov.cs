namespace Compiler.Common.Generation;

public record Mov : IInstruction
{
    private readonly string _value;
    
    public Mov(IOperand src, IOperand dest)
    {
        Src = src;
        Dest = dest.Type is OperationType.Immediate
            ? throw new ArgumentException("Destination cannot be a constant", nameof(dest))
            : dest;
        
        _value = $"movl {Src.Build()}, {Dest.Build()}";
    }

    public IOperand Src { get; }
    public IOperand Dest { get; }
    
    public string Build() => _value;
}