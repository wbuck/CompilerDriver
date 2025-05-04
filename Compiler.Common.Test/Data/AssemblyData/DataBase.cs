using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.AssemblyData;

public class DataBase : TheoryData<string, Program>
{
    protected static Program Create(List<IInstruction> instructions) =>
        new(new Function("main", instructions));
    
    protected static Imm<int> Imm(int value) =>
        new(value);
    
    protected static Stack Stack(int value) =>
        new(value);
    
    protected static AllocateStack AllocateStack(int value) =>
        new(value);
}