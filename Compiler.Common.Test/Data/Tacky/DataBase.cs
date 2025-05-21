using System.Runtime.InteropServices;
using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class DataBase : TheoryData<string, TackyProgram>
{
    protected static TackyProgram GetExpected(List<ITackyInstruction> instructions) =>
        new(new TackyFunction("main", instructions));

    protected static TackyConstant<int> Const(int value)
        => new(value);
    
    protected static TackyVariable Var(int varCount)
        => new($"tmp.{varCount}", varCount, varCount * Marshal.SizeOf<int>());

    protected static TackyLabel Label(string identifier)
        => new(identifier);
    
    protected static TackyJump Jump(string target)
        => new(target);
    
    protected static TackyReturn Ret(ITackyValue value)
        => new(value);
}