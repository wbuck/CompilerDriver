namespace Compiler.Common.Generation;

internal static class StackReplacer
{
    public static Program Replace(Program program)
        => new(Replace(program.Function));
    
    private static Function Replace(Function function)
        => new(function.Name, Replace(function.Instructions));
    

    private static List<IInstruction> Replace(List<IInstruction> instructions)
    {
        if (instructions.Count == 0 || 
            instructions.All(i => i is not Mov { Source: Stack, Destination: Stack }))
            return instructions;

        List<IInstruction> updated = new(instructions.Count);
        foreach (var instruction in instructions)
        {
            if (instruction is Mov { Source: Stack source, Destination: Stack dest })
            {
                updated.Add(new Mov(source, new R10()));
                updated.Add(new Mov(new R10(), dest));
                continue;
            }
            updated.Add(instruction);
        }
        return updated;
    }
}