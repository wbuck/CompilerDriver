using System.Runtime.InteropServices;
using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class DataBase : TheoryData<string, TackyProgram>
{
    protected static TackyProgram GetExpected(List<ITackyInstruction> instructions)
    {
        instructions.Add(new TackyReturn(new TackyConstant<int>(0)));
        return new TackyProgram(new TackyFunction("main", instructions));
    }

    protected static TackyConstant<int> Const(int value)
        => new(value);
    
    protected static TackyVariable Var(int varCount)
        => new($"tmp.{varCount}");
    
    protected static TackyVariable Var(string id)
        => new(id);

    protected static TackyLabel Label(string identifier)
        => new(identifier);
    
    protected static TackyJump Jump(string target)
        => new(target);
    
    protected static TackyReturn Ret(ITackyValue value)
        => new(value);        
    
    protected static string BreakTarget(string loopId)
        => $".break.{loopId}";
    
    protected static TackyJump BreakJump(string loopId)
        => Jump(BreakTarget(loopId));
    
    protected static TackyLabel BreakLabel(string loopId)
        => Label(BreakTarget(loopId));
    
    protected static string ContinueTarget(string loopId)
        => $".continue.{loopId}";
    
    protected static TackyLabel ContinueLabel(string loopId)
        => Label(ContinueTarget(loopId));    
    
    protected static TackyJump ContinueJump(string loopId)
        => Jump(ContinueTarget(loopId));
    
    protected static string BeginTarget(string loopId)
        => $".begin.{loopId}";
    protected static TackyLabel BeginLabel(string loopId)
        => Label(BeginTarget(loopId));
    
    protected static TackyJump BeginJump(string loopId)
        => Jump(BeginTarget(loopId));
}