namespace Compiler.Common.Generation;

internal class PseudoReplacer
{
    private int _offset;
    public int StackOffset
    {
        get => _offset;
        private set => _offset = Math.Max(_offset, value);
    }
    
    public Program Replace(Program program)
        => new(Replace(program.Function));

    private Function Replace(Function function)
        => new(function.Name, Replace(function.Instructions));
    
    private List<IInstruction> Replace(List<IInstruction> instructions)
        => instructions.Select(i => i switch
        {
            Mov mov => Replace(mov),
            Unary unary => Replace(unary),  
            Ret ret => ret,
            AllocateStack allocate => allocate,
            _ => throw new FormatException($"Unknown instruction type {i.GetType().Name}")
        }).ToList();
    
    private IInstruction Replace(Mov mov)
        => new Mov(Replace(mov.Source), Replace(mov.Destination));
    
    private IInstruction Replace(Unary unary)
        => new Unary(unary.Operator, Replace(unary.Operand));
    
    private IOperand Replace(IOperand operand)
        => operand is Pseudo pseudo
            ? new Stack(SetOffset(pseudo.StackOffset))
            : operand;

    private int SetOffset(int offset)
        => StackOffset = offset;
}