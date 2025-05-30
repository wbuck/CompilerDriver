using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.Assembly;

public class DataBase : TheoryData<string, Program>
{
    protected static Program GetExpected(List<IInstruction> instructions)
    {
        instructions.Add(new Mov(Zero, Ax.Register));
        instructions.Add(Ret.Instruction);
        return new Program(new Function("main", instructions));
    }
    
    protected static Imm<int> Imm(int value) =>
        new(value);

    protected static Imm<int> Zero { get; } = Imm(0);
    
    protected static Imm<int> One { get; } = Imm(1);
    
    protected static Stack Stack(int value) =>
        new(value);
    
    protected static AllocateStack AllocateStack(int value) =>
        new(value);
}