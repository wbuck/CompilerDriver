namespace Compiler.Common.Generation;

internal static class InvalidInstructionReplacer
{
    public static Program Replace(Program program)
        => new(ReplaceFunction(program.Function));
    
    private static Function ReplaceFunction(Function function)
        => new(function.Name, ReplaceInstructions(function.Instructions));
    
    private static List<IInstruction> ReplaceInstructions(List<IInstruction> instructions)
    {
        // if (instructions.Count == 0 || 
        //     instructions.All(i => i is not Mov { Source: Stack, Destination: Stack }))
        //     return instructions;

        List<IInstruction> updated = new(instructions.Count);
        foreach (var instruction in instructions)
        {
            switch (instruction)
            {
                case Mov { Source: Stack source, Destination: Stack dest }:
                    updated.Add(new Mov(source, R10.Register));
                    updated.Add(new Mov(R10.Register, dest));
                    break;
                case Div { Operand: Imm<int> constant }:
                    updated.Add(new Mov(constant, R10.Register));
                    updated.Add(new Div(R10.Register));
                    break;
                case Binary { Operator: Add or Sub, Source: Stack lhs, Destination: Stack rhs } addOrSub:
                    updated.Add(new Mov(lhs, R10.Register));
                    updated.Add(new Binary(addOrSub.Operator, R10.Register, rhs));
                    break;
                case Binary { Operator: Mult, Destination: Stack rhs } mult:
                    updated.Add(new Mov(rhs, R11.Register));
                    updated.Add(new Binary(mult.Operator, mult.Source, R11.Register));
                    updated.Add(new Mov(R11.Register, mult.Destination));
                    break;
                default:
                    updated.Add(instruction);
                    break;
            }
        }
        return updated;
    }
}