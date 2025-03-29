namespace Compiler.Common.Generation;

public interface IOperand : IBuild
{
    public OperationType Type { get; }
}