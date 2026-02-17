using Compiler.Tacky.Helpers;
using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class LocalVariableData : DataBase
{
    public LocalVariableData()
    {
        Add
        (
            """
            int main(void) {
                int first_variable = 1;
                int second_variable = 2;
                return first_variable + second_variable;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("first_variable.0")),
                new TackyCopy(Const(2), Var("second_variable.1")),
                new TackyBinary
                (
                    TackyAddition.Operator, 
                    Var("first_variable.0"),
                    Var("second_variable.1"),
                    Var(1)
                ),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 2147483646;
                int b = 0;
                int c = a / 6 + !b;
                return c * 2 == a - 1431655762;
            }
            """,
            GetExpected([
                new TackyCopy(Const(2147483646), Var("a.0")),
                new TackyCopy(Const(0), Var("b.1")),
                new TackyBinary
                (
                    TackyDivision.Operator, 
                    Var("a.0"),
                    Const(6),
                    Var(1)
                ),
                new TackyUnary(TackyNot.Operator, Var("b.1"), Var(2)),
                new TackyBinary
                (
                    TackyAddition.Operator, 
                    Var(1),
                    Var(2),
                    Var(3)
                ),
                new TackyCopy(Var(3), Var("c.2")),
                new TackyBinary
                (
                    TackyMultiplication.Operator, 
                    Var("c.2"),
                    Const(2),
                    Var(4)
                ),
                new TackyBinary
                (
                    TackySubtraction.Operator, 
                    Var("a.0"),
                    Const(1431655762),
                    Var(5)
                ),
                new TackyBinary
                (
                    TackyEqual.Operator, 
                    Var(4),
                    Var(5),
                    Var(6)
                ),
                Ret(Var(6))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = a = 5;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(5), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int var0;
                var0 = 2;
                return var0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(2), Var("var0.0")),
                Ret(Var("var0.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a;
                int b = a = 0;
                return b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("b.1")),
                Ret(Var("b.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a;
                a = 0 || 5;
                return a;
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Const(5), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyCopy(Var(1), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
            
            }
            """,
            GetExpected(new List<ITackyInstruction>())
        );
        Add
        (
            """
            int main(void) {
                int a = -2593;
                a = a % 3;
                int b = -a;
                return b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(-2593), Var("a.0")),
                new TackyBinary
                (
                    TackyRemainder.Operator,
                    Var("a.0"),
                    Const(3),
                    Var(1)
                ),
                new TackyCopy(Var(1), Var("a.0")),
                new TackyUnary(TackyNegate.Operator, Var("a.0"), Var(2)),
                new TackyCopy(Var(2), Var("b.1")),
                Ret(Var("b.1"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int return_val = 3;
                int void2 = 2;
                return return_val + void2;
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("return_val.0")),
                new TackyCopy(Const(2), Var("void2.1")),
                new TackyBinary
                (
                    TackyAddition.Operator, 
                    Var("return_val.0"),
                    Var("void2.1"),
                    Var(1)
                ),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 3;
                a = a + 5;
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("a.0")),
                new TackyBinary
                (
                    TackyAddition.Operator,
                    Var("a.0"),
                    Const(5),
                    Var(1)
                ),
                new TackyCopy(Var(1), Var("a.0")),
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 0;
                a = 3 * (b = a);
                return a + b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Const(0), Var("b.1")),
                new TackyCopy(Var("a.0"), Var("b.1")),
                new TackyBinary
                (
                    TackyMultiplication.Operator,
                    Const(3),
                    Var("b.1"),
                    Var(1)
                ),
                new TackyCopy(Var(1), Var("a.0")),
                new TackyBinary
                (
                    TackyAddition.Operator,
                    Var("a.0"),
                    Var("b.1"),
                    Var(2)
                ),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 || (a = 1);
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("a.0")),
                new TackyJumpIfNotZero(Var("a.0"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                ;
            }
            """,
            GetExpected(new List<ITackyInstruction>())
        );
        Add
        (
            """
            int main(void) {
                ;
                return 0;
            }
            """,
            GetExpected([Ret(Const(0))])
        );
        Add
        (
            """
            int main(void) {
                int a = 2;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(2), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 && (a = 5);
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(5), Var("a.0")),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Jump($".{TackyConstants.AND_END_LABEL}2"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Label($".{TackyConstants.AND_END_LABEL}2"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                1 || (a = 1);
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyJumpIfNotZero(Const(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("a.0")),
                new TackyJumpIfNotZero(Var("a.0"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                2 + 2;
                return 0;
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(2), Const(2), Var(1)),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {            
                int a = 1;
                int b = 2;
                return a = b = 4;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Const(2), Var("b.1")),
                new TackyCopy(Const(4), Var("b.1")),
                new TackyCopy(Var("b.1"), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0 && a;
                return a;
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyJumpIfZero(Var("a.0"), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Jump($".{TackyConstants.AND_END_LABEL}2"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Label($".{TackyConstants.AND_END_LABEL}2"),
                new TackyCopy(Var(1), Var("a.0")),
                Ret(Var("a.0"))
            ])
        );
    }
}