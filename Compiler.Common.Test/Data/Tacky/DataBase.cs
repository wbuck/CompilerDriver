using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class DataBase : TheoryData<string, TackyProgram>
{
    protected static TackyProgram Create(List<ITackyInstruction> instructions) =>
        new(new TackyFunction("main", instructions));

    protected static TackyConstant<int> Constant(int value)
        => new(value);
    
    protected static TackyVariable Variable(int varCount)
        => new(varCount);
}