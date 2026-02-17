using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class CompoundStatementData : DataBase
{
    public CompoundStatementData()
    {
        Add
        (
            """
            int main(void) {
                int a = 3;
                {
                    int a = a = 4;
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("a.0")),
                new TackyCopy(Const(4), Var("a.1")),
                new TackyCopy(Var("a.1"), Var("a.1")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                {
                    int a = a = 4;
                    return a;
                }
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("a.0")),
                new TackyCopy(Const(4), Var("a.1")),
                new TackyCopy(Var("a.1"), Var("a.1")),
                Ret(Var("a.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a;
                {
                    int b = a = 1;
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("b.1")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int ten = 10;
                {}
                int twenty = 10 * 2;
                {{}}
                return ten + twenty;
            }
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("ten.0")),                
                new TackyCopy(Const(20), Var("twenty.1")),
                new TackyBinary(TackyAddition.Operator, Var("ten.0"), Var("twenty.1"), Var("tmp.1")),
                Ret(Var("tmp.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b;
                {
                    a = -4;
                    int a = 7;
                    b = a + 1;
                }
                return b == 8 && a == -4;
            }
            """,
            GetExpected([
                new TackyCopy(Const(2), Var("a.0")),
                new TackyUnary(TackyNegate.Operator, Const(4), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("a.0")),
                new TackyCopy(Const(7), Var("a.2")),
                new TackyBinary(TackyAddition.Operator, Var("a.2"), Const(1), Var("tmp.2")),
                new TackyCopy(Var("tmp.2"), Var("b.1")),
                new TackyBinary(TackyEqual.Operator, Var("b.1"), Const(8), Var("tmp.3")),
                new TackyJumpIfZero(Var("tmp.3"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyUnary(TackyNegate.Operator, Const(4), Var("tmp.4")),
                new TackyBinary(TackyEqual.Operator, Var("a.0"), Var("tmp.4"), Var("tmp.5")),
                new TackyJumpIfZero(Var("tmp.5"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.6")),
                Jump($".{TackyConstants.AND_END_LABEL}2"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.6")),
                Label($".{TackyConstants.AND_END_LABEL}2"),
                Ret(Var("tmp.6"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                {
                    int a = 1;
                    return a;
                }
            }
            """,
            GetExpected([
                new TackyCopy(Const(2), Var("a.0")),
                new TackyCopy(Const(1), Var("a.1")),
                Ret(Var("a.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int x = 4;
                {
                    int x;
                }
                return x;
            }
            """,
            GetExpected([
                new TackyCopy(Const(4), Var("x.0")),
                Ret(Var("x.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                {
                    int b = 4;
                    a = b;
                }
                {
                    int b = 2;
                    a = a - b;
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Const(4), Var("b.1")),
                new TackyCopy(Var("b.1"), Var("a.0")),
                new TackyCopy(Const(2), Var("b.2")),
                new TackyBinary(TackySubtraction.Operator, Var("a.0"), Var("b.2"), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                if (a) {
                    int b = 2;
                    return b;
                } else {
                    int c = 3;
                    if (a < c) {
                        return !a;
                    } else {
                        return 5;
                    }
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.ELSE_LABEL}2"),
                new TackyCopy(Const(2), Var("b.1")),
                Ret(Var("b.1")),
                Jump($".{TackyConstants.IF_END_LABEL}1"),
                Label($".{TackyConstants.ELSE_LABEL}2"),
                new TackyCopy(Const(3), Var("c.2")),
                new TackyBinary(TackyLessThan.Operator, Var("a.0"), Var("c.2"), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.ELSE_LABEL}4"),
                new TackyUnary(TackyNot.Operator, Var("a.0"), Var("tmp.2")),
                Ret(Var("tmp.2")),
                Jump($".{TackyConstants.IF_END_LABEL}3"),
                Label($".{TackyConstants.ELSE_LABEL}4"),
                Ret(Const(5)),
                Label($".{TackyConstants.IF_END_LABEL}3"),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void)
            {
                int x;
                {
                    x = 3;
                }
                {
                    return x;
                }
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("x.0")),
                Ret(Var("x.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 5;
                if (a > 4) {
                    a -= 4;
                    int a = 5;
                    if (a > 4) {
                        a -= 4;
                    }
                }
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(5), Var("a.0")),
                new TackyBinary(TackyGreaterThan.Operator, Var("a.0"), Const(4), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.IF_END_LABEL}1"),
                new TackyBinary(TackySubtraction.Operator, Var("a.0"), Const(4), Var("a.0")),
                new TackyCopy(Const(5), Var("a.1")),
                new TackyBinary(TackyGreaterThan.Operator, Var("a.1"), Const(4), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.IF_END_LABEL}2"),
                new TackyBinary(TackySubtraction.Operator, Var("a.1"), Const(4), Var("a.1")),
                Label($".{TackyConstants.IF_END_LABEL}2"),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                {
                    if (a != 0)
                        return_a:
                            return a;
                    int a = 4;
                    goto return_a;
                }
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyBinary(TackyNotEqual.Operator, Var("a.0"), Const(0), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.IF_END_LABEL}1"),
                Label(".return_a1"),
                Ret(Var("a.0")),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                new TackyCopy(Const(4), Var("a.1")),
                Jump(".return_a1")
            ])
        );
        Add
        (
            """
            int main(void) {
                int x = 5;
                goto inner;
                {
                    int x = 0;
                    inner:
                    x = 1;
                    return x;
                }
            }
            """,
            GetExpected([
                new TackyCopy(Const(5), Var("x.0")),
                new TackyJump(".inner1"),
                new TackyCopy(Const(0), Var("x.1")),
                new TackyLabel(".inner1"),
                new TackyCopy(Const(1), Var("x.1")),
                Ret(Var("x.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 10;
                int b = 0;
                if (a) {
                    int a = 1;
                    b = a;
                    goto end;
                }
                a = 9;
            end:
                return (a == 10 && b == 1);
            }
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("a.0")),
                new TackyCopy(Const(0), Var("b.1")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.IF_END_LABEL}1"),
                new TackyCopy(Const(1), Var("a.2")),
                new TackyCopy(Var("a.2"), Var("b.1")),
                Jump(".end1"),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                new TackyCopy(Const(9), Var("a.0")),
                Label(".end1"),
                new TackyBinary(TackyEqual.Operator, Var("a.0"), Const(10), Var("tmp.1")),
                new TackyJumpIfZero(Var("tmp.1"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyBinary(TackyEqual.Operator, Var("b.1"), Const(1), Var("tmp.2")),
                new TackyJumpIfZero(Var("tmp.2"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(1), Var("tmp.3")),
                Jump($".{TackyConstants.AND_END_LABEL}3"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(0), Var("tmp.3")),
                Label($".{TackyConstants.AND_END_LABEL}3"),
                Ret(Var("tmp.3"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int sum = 0;
                if (1) {
                    int a = 5;
                    goto other_if;
                    sum = 0;
                first_if:                   
                    a = 5;
                    sum = sum + a;
                }
                if (0) {
                other_if:;
                    int a = 6;
                    sum = sum + a;
                    goto first_if;
                    sum = 0;
                }
                return sum;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("sum.0")),
                new TackyJumpIfZero(Const(1), $".{TackyConstants.IF_END_LABEL}1"),
                new TackyCopy(Const(5), Var("a.1")),
                Jump(".other_if1"),
                new TackyCopy(Const(0), Var("sum.0")),
                Label(".first_if2"),
                new TackyCopy(Const(5), Var("a.1")),
                new TackyBinary(TackyAddition.Operator, Var("sum.0"), Var("a.1"), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("sum.0")),
                Label($".{TackyConstants.IF_END_LABEL}1"),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.IF_END_LABEL}2"),
                Label(".other_if1"),
                new TackyCopy(Const(6), Var("a.2")),
                new TackyBinary(TackyAddition.Operator, Var("sum.0"), Var("a.2"), Var("tmp.2")),
                new TackyCopy(Var("tmp.2"), Var("sum.0")),
                Jump(".first_if2"),
                new TackyCopy(Const(0), Var("sum.0")),
                Label($".{TackyConstants.IF_END_LABEL}2"),
                Ret(Var("sum.0"))
            ])
        );
    }
}