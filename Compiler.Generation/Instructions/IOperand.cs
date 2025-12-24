namespace Compiler.Generation.Instructions;

public interface IOperand : IAssembly
{
    static Imm<int> Zero { get; } = new(0);
}