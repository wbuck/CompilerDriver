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
        => new(ReplaceFunction(program.Function));

    private Function ReplaceFunction(Function function)
        => function with { Instructions = ReplaceInstructions(function.Instructions) };

    private List<IInstruction> ReplaceInstructions(List<IInstruction> instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            instructions[i] = instructions[i] switch
            {
                Mov mov => ReplaceMov(mov),
                Unary unary => ReplaceUnary(unary),
                Binary binary => ReplaceBinary(binary),
                Div div => ReplaceDiv(div),
                Ret ret => ret,
                Cdq cdq => cdq,
                AllocateStack allocate => allocate,
                _ => throw new FormatException($"Unknown instruction type {instructions[i].GetType().Name}")
            };
        }
        return instructions;
    }

    private Mov ReplaceMov(Mov mov)
        => IsPseudo(mov.Source) || IsPseudo(mov.Destination)
            ? new Mov(ReplaceOperand(mov.Source), ReplaceOperand(mov.Destination))
            : mov;

    private Unary ReplaceUnary(Unary unary)
        => IsPseudo(unary.Operand)
            ? unary with { Operand = ReplaceOperand(unary.Operand) }
            : unary;

    private Binary ReplaceBinary(Binary binary)
        => IsPseudo(binary.Source) || IsPseudo(binary.Destination)
            ? binary with { Source = ReplaceOperand(binary.Source), Destination = ReplaceOperand(binary.Destination) }
            : binary;

    private Div ReplaceDiv(Div div)
        => IsPseudo(div.Operand)
            ? new Div(Operand: ReplaceOperand(div.Operand))
            : div;
    
    private IOperand ReplaceOperand(IOperand operand)
        => operand is Pseudo pseudo
            ? new Stack(SetOffset(pseudo.StackOffset))
            : operand;
    
    private static bool IsPseudo(IOperand operand)
        => operand is Pseudo;

    private int SetOffset(int offset)
        => StackOffset = offset;
}