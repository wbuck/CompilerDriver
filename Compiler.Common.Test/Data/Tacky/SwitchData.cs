using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class SwitchData : DataBase
{
    public SwitchData()
    {
        Add
        (
            """
            int main(void) {
                while(1) {
                    switch (1) {
                        case 1: break;
                        case 2: continue;
                    }
                }
            }
            """,
            GetExpected([
                Label(ContinueTarget("while1")),
                new TackyJumpIfZero(Const(1), BreakTarget("while1")),
                new TackyBinary(TackyEqual.Operator, Const(2), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case3"),
                new TackyBinary(TackyEqual.Operator, Const(1), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case2"),
                Label(".switch1.case2"),
                Jump(BreakTarget("switch1")),
                Label(".switch1.case3"),
                Jump(ContinueTarget("while1")),
                Label(BreakTarget("switch1")),
                Jump(ContinueTarget("while1")),
                Label(BreakTarget("while1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case 1:
                        switch (2) {
                            case 2: break;
                            default: break;
                        }
                        break;
                    default: break;
                }
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(1), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case2"),
                Jump(".switch1.default"),
                Label(".switch1.case2"),
                new TackyBinary(TackyEqual.Operator, Const(2), Const(2), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), ".switch3.case4"),
                Jump(".switch3.default"),
                Label(".switch3.case4"),
                Jump(BreakTarget("switch3")),
                Label(".switch3.default"),
                Jump(BreakTarget("switch3")),
                Label(BreakTarget("switch3")),
                Jump(BreakTarget("switch1")),
                Label(".switch1.default"),
                Jump(BreakTarget("switch1")),
                Label(BreakTarget("switch1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                switch (1);
            }
            """,
            GetExpected([])
        );
        Add
        (
            """
            int main(void) {
                switch (1) {
                    case 5 - 4: break;
                }
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(1), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case2"),
                Label(".switch1.case2"),
                Jump(BreakTarget("switch1")),
                Label(BreakTarget("switch1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                switch (1) {
                    case 1:
                        a = 1;
                        break;
                     case 2:
                        a = 2;
                        break;
                     default:
                        a = 10;
                        break; 
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyBinary(TackyEqual.Operator, Const(2), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case3"),
                new TackyBinary(TackyEqual.Operator, Const(1), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case2"),
                Jump(".switch1.default"),
                Label(".switch1.case2"),
                new TackyCopy(Const(1), Var("a.0")),
                Jump(BreakTarget("switch1")),
                Label(".switch1.case3"),
                new TackyCopy(Const(2), Var("a.0")),
                Jump(BreakTarget("switch1")),
                Label(".switch1.default"),
                new TackyCopy(Const(10), Var("a.0")),
                Jump(BreakTarget("switch1")),
                Label(BreakTarget("switch1")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                switch(3) {
                    case 0: return 0;
                    case 1: return 1;
                    case 3: return 3;
                    case 5: return 5;        
                }
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(5), Const(3), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case5"),
                new TackyBinary(TackyEqual.Operator, Const(3), Const(3), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case4"),
                new TackyBinary(TackyEqual.Operator, Const(1), Const(3), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case3"),
                new TackyBinary(TackyEqual.Operator, Const(0), Const(3), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), ".switch1.case2"),
                Label(".switch1.case2"),
                Ret(Const(0)),
                Label(".switch1.case3"),
                Ret(Const(1)),
                Label(".switch1.case4"),
                Ret(Const(3)),
                Label(".switch1.case5"),
                Ret(Const(5)),
                Label(BreakTarget("switch1"))
            ])
        );
    }
}