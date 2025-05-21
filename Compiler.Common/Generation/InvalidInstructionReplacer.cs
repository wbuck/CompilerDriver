namespace Compiler.Common.Generation;

internal static class InvalidInstructionReplacer
{
    public static Program Replace(Program program)
        => new(ReplaceFunction(program.Function));
    
    private static Function ReplaceFunction(Function function)
        => new(function.Name, ReplaceInstructions(function.Instructions));
    
    private static List<IInstruction> ReplaceInstructions(List<IInstruction> instructions)
    {
        List<IInstruction> updated = new(instructions.Count);
        foreach (var instruction in instructions)
        {
            switch (instruction)
            {
                case Mov { Source: Stack source, Destination: Stack dest }:
                    updated.Add(new Mov(source, R10.Register));
                    updated.Add(new Mov(R10.Register, dest));
                    break;
                case Div { Operand: IConstant constant }:
                    updated.Add(new Mov(constant, R10.Register));
                    updated.Add(new Div(R10.Register));
                    break;
                case Binary { Operator: Add or Sub, Source: Stack source, Destination: Stack dest } addOrSub:
                    updated.Add(new Mov(source, R10.Register));
                    updated.Add(new Binary(addOrSub.Operator, R10.Register, dest));
                    break;
                case Binary { Operator: Mult, Destination: Stack dest } mult:
                    updated.Add(new Mov(dest, R11.Register));
                    updated.Add(mult with { Destination = R11.Register });
                    updated.Add(new Mov(R11.Register, mult.Destination));
                    break;
                case Binary { Operator: Add or Sub or Mult, Destination: IConstant } binary:
                    updated.Add(new Mov(binary.Destination, R11.Register));
                    updated.Add(binary with { Destination = R11.Register });
                    break;
                case Bitwise { Operator: BitwiseAnd or BitwiseOr or BitwiseXor, Source: Stack source, Destination: Stack dest } andOrXor:
                    updated.Add(new Mov(source, R10.Register));
                    updated.Add(new Bitwise(andOrXor.Operator, R10.Register, dest));
                    break;
                case Bitwise { Operator: BitwiseLeftShift or BitwiseRightShift, Source: Stack source } shift:
                    updated.Add(new Mov(source, Cx.Register));
                    updated.Add(shift with { Source = Cx.Register });
                    break;
                case Cmp { Lhs: Stack lhs, Rhs: Stack rhs }:
                    updated.Add(new Mov(lhs, R10.Register));
                    updated.Add(new Cmp(R10.Register, rhs));
                    break;
                case Cmp { Rhs: IConstant } cmp:
                    updated.Add(new Mov(cmp.Rhs, R11.Register));
                    updated.Add(cmp with { Rhs = R11.Register });
                    break;
                default:
                    updated.Add(instruction);
                    break;
            }
        }
        return updated;
    }
}