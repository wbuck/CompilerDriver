using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class DataBase : TheoryData<string, TackyProgram>
{
    protected static TackyProgram GetExpected(List<ITackyInstruction> instructions)
    {
        instructions.Add(new TackyReturn(new TackyConstant<int>(0)));
        return new TackyProgram([new TackyFunction("main", true, [], instructions)]);
    }

    protected static TackyProgram GetExpected(List<ITackyTopLevel> topLevel)
        => new(topLevel);

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
    
    protected static string BreakTarget(string id)
        => $".break.{id}";
    
    protected static TackyJump BreakJump(string id)
        => Jump(BreakTarget(id));
    
    protected static TackyLabel BreakLabel(string id)
        => Label(BreakTarget(id));
    
    protected static string ContinueTarget(string id)
        => $".continue.{id}";
    
    protected static TackyLabel ContinueLabel(string id)
        => Label(ContinueTarget(id));    
    
    protected static TackyJump ContinueJump(string id)
        => Jump(ContinueTarget(id));
    
    protected static string BeginTarget(string id)
        => $".begin.{id}";
    protected static TackyLabel BeginLabel(string id)
        => Label(BeginTarget(id));
    
    protected static TackyJump BeginJump(string loopId)
        => Jump(BeginTarget(loopId));
}