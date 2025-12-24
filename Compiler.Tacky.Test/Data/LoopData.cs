using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class LoopData : DataBase
{
    public LoopData()
    {
        Add
        (
            """
            int main(void) {
                int a = 10;
                while ((a = 1))
                    break;
                return a;
            }            
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("a.0")),
                ContinueLabel("while1"),
                new TackyCopy(Const(1), Var("a.0")),
                new TackyJumpIfZero(Var("a.0"), BreakTarget("while1")),
                BreakJump("while1"),
                ContinueJump("while1"),
                BreakLabel("while1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                do {
                    a *= 2;
                } while(a < 11);            
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                BeginLabel("do_while1"),
                new TackyBinary(TackyMultiplication.Operator, Var("a.0"), Const(2), Var("a.0")),
                ContinueLabel("do_while1"),
                new TackyBinary(TackyLessThan.Operator, Var("a.0"), Const(11), Var("tmp.1")),
                new TackyJumpIfNotZero(Var("tmp.1"), BeginTarget("do_while1")),
                BreakLabel("do_while1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
            
                for (int i = -100; i <= 0; i++)
                    a++;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyUnary(TackyNegate.Operator, Const(100), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("i.1")),
                BeginLabel("for1"),
                new TackyBinary(TackyLessThanOrEqual.Operator, Var("i.1"), Const(0), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), BreakTarget("for1")),
                new TackyCopy(Var("a.0"), Var("tmp.3")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                ContinueLabel("for1"),
                new TackyCopy(Var("i.1"), Var("tmp.4")),
                new TackyBinary(TackyAddition.Operator, Var("i.1"), Const(1), Var("i.1")),
                BeginJump("for1"),
                BreakLabel("for1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                for (int i = 0; i <= 10; ++i) {
                    if (i % 2 == 0) {
                        continue;
                    }
                }
                return 42;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("i.0")),
                BeginLabel("for1"),
                new TackyBinary(TackyLessThanOrEqual.Operator, Var("i.0"), Const(10), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), BreakTarget("for1")),
                new TackyBinary(TackyRemainder.Operator, Var("i.0"), Const(2), Var("tmp.2")),
                new TackyBinary(TackyEqual.Operator, Var("tmp.2"), Const(0), Var("tmp.3")),
                new TackyJumpIfZero(Var("tmp.3"), $".{TackyConstants.IF_END_LABEL}1"),
                Jump(ContinueTarget("for1")),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Label(ContinueTarget("for1")),
                new TackyBinary(TackyAddition.Operator, Var("i.0"), Const(1), Var("i.0")),
                Jump(BeginTarget("for1")),
                Label(BreakTarget("for1")),
                Ret(Const(42))
            ])
        );
    }
}