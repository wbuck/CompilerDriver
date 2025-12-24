using System.Runtime.InteropServices;
using Compiler.Generation.Instructions;

namespace Compiler.Generation.Helpers;

internal class PseudoReplacer
{
    private readonly Dictionary<string, int> _offsets = [];
    public int StackOffset { get; private set; }

    public Function Replace(Function function)
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
                Bitwise bitwise => ReplaceBitwise(bitwise),
                Cmp cmp => ReplaceCmp(cmp),
                SetConditional set => ReplaceSetCc(set),
                Push push => ReplacePush(push),
                Call call => call,
                Jmp jmp => jmp,
                JmpConditional jmp => jmp,
                Label label => label,
                Ret ret => ret,
                Cdq cdq => cdq,
                AllocateStack allocate => allocate,
                DeallocateStack deallocate => deallocate,
                _ => throw new FormatException($"Unknown instruction type {instructions[i].Tag.ToStringFast()}")
            };
        }
        return instructions;
    }
    
    private Push ReplacePush(Push push)
        => IsPseudo(push.Operand)
            ? push with { Operand = ReplaceOperand(push.Operand) }
            : push;
    
    private SetConditional ReplaceSetCc(SetConditional setConditional)
        => IsPseudo(setConditional.Operand)
            ? setConditional with { Operand = ReplaceOperand(setConditional.Operand) }
            : setConditional;

    private Cmp ReplaceCmp(Cmp cmp)
        => IsPseudo(cmp.Destination) || IsPseudo(cmp.Source)
            ? new Cmp(ReplaceOperand(cmp.Source), ReplaceOperand(cmp.Destination))
            : cmp;
    
    private Bitwise ReplaceBitwise(Bitwise bitwise)
        => IsPseudo(bitwise.Source) || IsPseudo(bitwise.Destination)
            ? bitwise with { Source = ReplaceOperand(bitwise.Source), Destination = ReplaceOperand(bitwise.Destination) }
            : bitwise;

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
    {
        if (operand is not Pseudo pseudo)
            return operand;
        
        if (_offsets.TryGetValue(pseudo.Identifier, out var offset))
            return new Stack(-offset);

        StackOffset += Marshal.SizeOf<int>();
        _offsets[pseudo.Identifier] = StackOffset;
        
        return new Stack(-StackOffset);
    }
    
    private static bool IsPseudo(IOperand operand)
        => operand is Pseudo;
}