using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class IfStatementAndConditionalExpressionData : DataBase
{
    public IfStatementAndConditionalExpressionData()
    {
        Add
        (
            """
            int main(void) {
                int a = 0;
                a = 1 ? 2 : 3;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfZero(Const(1), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(2), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(3), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                new TackyCopy(Var("tmp.1"),  Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                if (1 + 2 == 3)
                    return 5;
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(1), Const(2), Var("tmp.1")),
                new TackyBinary(TackyEqual.Operator, Var("tmp.1"), Const(3), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.IF_END_LABEL}1"),
                Ret(Const(5)),
                Label($".{TackyConstants.IF_END_LABEL}1")                
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if (a)
                    return 1;
                else
                    return 2;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.ELSE_LABEL}2"),
                Ret(Const(1)),
                new TackyJump($".{TackyConstants.IF_END_LABEL}1"),
                Label($".{TackyConstants.ELSE_LABEL}2"),
                Ret(Const(2)),
                Label($".{TackyConstants.IF_END_LABEL}1")
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 1;
                if (a)
                    b = 1;
                else if (~b)
                    b = 2;
                return b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Const(1), Var("b.1")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.ELSE_LABEL}2"),
                new TackyCopy(Const(1), Var("b.1")),
                Jump($".{TackyConstants.IF_END_LABEL}1"),
                Label($".{TackyConstants.ELSE_LABEL}2"),
                new TackyUnary(TackyComplement.Operator, Var("b.1"), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.IF_END_LABEL}3"),
                new TackyCopy(Const(2), Var("b.1")),
                Label($".{TackyConstants.IF_END_LABEL}3"),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("b.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if ( (a = 1) )
                    if (a == 1)
                        a = 3;
                    else
                        a = 4;
            
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Const(1), Var("a.0")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.IF_END_LABEL}1"),
                new TackyBinary(TackyEqual.Operator, Var("a.0"), Const(1), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.ELSE_LABEL}3"),
                new TackyCopy(Const(3), Var("a.0")),
                Jump($".{TackyConstants.IF_END_LABEL}2"),
                Label($".{TackyConstants.ELSE_LABEL}3"),
                new TackyCopy(Const(4), Var("a.0")),                
                Label($".{TackyConstants.IF_END_LABEL}2"),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("a.0")),
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if (0)
                    if (0)
                        a = 3;
                    else
                        a = 4;
                else
                    a = 1;
            
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.ELSE_LABEL}2"),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.ELSE_LABEL}4"),
                new TackyCopy(Const(3), Var("a.0")),
                Jump($".{TackyConstants.IF_END_LABEL}3"),
                Label($".{TackyConstants.ELSE_LABEL}4"),
                new TackyCopy(Const(4), Var("a.0")),
                Label($".{TackyConstants.IF_END_LABEL}3"),
                Jump($".{TackyConstants.IF_END_LABEL}1"),
                Label($".{TackyConstants.ELSE_LABEL}2"),
                new TackyCopy(Const(1), Var("a.0")),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int x = 0;
                if (0)
                    ;
                else
                    x = 1;
                return x;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("x.0")),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.ELSE_LABEL}2"),
                Jump($".{TackyConstants.IF_END_LABEL}1"),
                Label($".{TackyConstants.ELSE_LABEL}2"),
                new TackyCopy(Const(1), Var("x.0")),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("x.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int x = 10;
                int y = 0;
                y = (x = 5) ? x : 2;
                return y;
            }
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("x.0")),
                new TackyCopy(Const(0), Var("y.1")),
                new TackyCopy(Const(5), Var("x.0")),
                new TackyJumpIfZero(Var("x.0"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Var("x.0"), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(2), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                new TackyCopy(Var("tmp.1"), Var("y.1")),
                Ret(Var("y.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int flag = 0;           
                return a > b ? 5 : flag ? 6 : 7;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Const(2), Var("b.1")),
                new TackyCopy(Const(0), Var("flag.2")),
                new TackyBinary(TackyGreaterThan.Operator, Var("a.0"), Var("b.1"), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(5), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyJumpIfZero(Var("flag.2"), $".{TackyConstants.CONDITION_ELSE_LABEL}3"),
                new TackyCopy(Const(6), Var("tmp.3")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}4"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}3"),
                new TackyCopy(Const(7), Var("tmp.3")),
                Label($".{TackyConstants.CONDITION_END_LABEL}4"),
                new TackyCopy(Var("tmp.3"), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("tmp.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int flag = 1;
                int a = 0;
                flag ? a = 1 : (a = 0);
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("flag.0")),
                new TackyCopy(Const(0), Var("a.1")),
                new TackyJumpIfZero(Var("flag.0"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(1), Var("a.1")),
                new TackyCopy(Var("a.1"), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(0), Var("a.1")),
                new TackyCopy(Var("a.1"), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("a.1"))                
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a != 2 ? a = 2 : 0;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyBinary(TackyNotEqual.Operator, Var("a.0"), Const(2), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(2), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1 ? 3 % 2 : 4;
                return a;
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(1), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyBinary(TackyRemainder.Operator, Const(3), Const(2), Var("tmp.2")),
                new TackyCopy(Var("tmp.2"), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(4), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                new TackyCopy(Var("tmp.1"), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                return a || 0 ? 20 : 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("a.0")),
                new TackyJumpIfNotZero(Var("a.0"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(0), Var("tmp.2")),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(1), Var("tmp.2")),
                Label($".{TackyConstants.OR_END_LABEL}4"),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(20), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("tmp.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 ? 1 : 0 || 2;
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(0),  $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyJumpIfNotZero(Const(2),  $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(0), Var("tmp.2")),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(1), Var("tmp.2")),
                Label($".{TackyConstants.OR_END_LABEL}4"),
                new TackyCopy(Var("tmp.2"), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("tmp.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
                a ? (b = 1) : (b = 2);
                return b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Const(0),  Var("b.1")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(1), Var("b.1")),
                new TackyCopy(Var("b.1"), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(2), Var("b.1")),
                new TackyCopy(Var("b.1"), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("b.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                return a > -1 ? 4 : 5;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyUnary(TackyNegate.Operator, Const(1), Var("tmp.2")),
                new TackyBinary(TackyGreaterThan.Operator, Var("a.0"), Var("tmp.2"), Var("tmp.3")),
                new TackyJumpIfZero(Var("tmp.3"),  $".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(4), Var("tmp.1")),
                Jump($".{TackyConstants.CONDITION_END_LABEL}2"),
                Label($".{TackyConstants.CONDITION_ELSE_LABEL}1"),
                new TackyCopy(Const(5), Var("tmp.1")),
                Label($".{TackyConstants.CONDITION_END_LABEL}2"),
                Ret(Var("tmp.1"))
            ])
        );
    }
}