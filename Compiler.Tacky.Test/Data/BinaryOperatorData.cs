using Compiler.Tacky.Tac;

namespace Compiler.Tacky.Test.Data;

public class BinaryOperatorData : DataBase
{
    public BinaryOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 1 + 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(1), Const(2), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 6 / 3 / 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyDivision.Operator, Const(6), Const(3), Var(1)),
                new TackyBinary(TackyDivision.Operator, Var(1), Const(2), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 * 4 / 2 -
                    3 % (2 + 1);
            }
            """,
            GetExpected([
                new TackyBinary(TackyMultiplication.Operator, Const(5), Const(4), Var(1)),
                new TackyBinary(TackyDivision.Operator, Var(1), Const(2), Var(2)),
                new TackyBinary(TackyAddition.Operator, Const(2), Const(1), Var(3)),
                new TackyBinary(TackyRemainder.Operator, Const(3), Var(3), Var(4)),
                new TackyBinary(TackySubtraction.Operator, Var(2), Var(4), Var(5)),
                new TackyReturn(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (3 / 2 * 4) + (5 - 4 + 3);
            }
            """,
            GetExpected([
                new TackyBinary(TackyDivision.Operator, Const(3), Const(2), Var(1)),
                new TackyBinary(TackyMultiplication.Operator, Var(1), Const(4), Var(2)),
                new TackyBinary(TackySubtraction.Operator, Const(5), Const(4), Var(3)),
                new TackyBinary(TackyAddition.Operator, Var(3), Const(3), Var(4)),
                new TackyBinary(TackyAddition.Operator, Var(2), Var(4), Var(5)),
                new TackyReturn(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2 - 3;
            }
            """,
            GetExpected([
                new TackyBinary(TackySubtraction.Operator, Const(1), Const(2), Var(1)),
                new TackyBinary(TackySubtraction.Operator, Var(1), Const(3), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (-12) / 5;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(12), Var(1)),
                new TackyBinary(TackyDivision.Operator, Var(1), Const(5), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 / 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyDivision.Operator, Const(4), Const(2), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 % 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyRemainder.Operator, Const(4), Const(2), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * 3;
            }
            """,
            GetExpected([
                new TackyBinary(TackyMultiplication.Operator, Const(2), Const(3), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * (3 + 4);
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(3), Const(4), Var(1)),
                new TackyBinary(TackyMultiplication.Operator, Const(2), Var(1), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 + 3 * 4;
            }
            """,
            GetExpected([                
                new TackyBinary(TackyMultiplication.Operator, Const(3), Const(4), Var(1)),
                new TackyBinary(TackyAddition.Operator, Const(2), Var(1), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2- -1;
            }
            """,
            GetExpected([                
                new TackyUnary(TackyNegate.Operator, Const(1), Var(1)),
                new TackyBinary(TackySubtraction.Operator, Const(2), Var(1), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackySubtraction.Operator, Const(1), Const(2), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 + 3;
            }
            """,
            GetExpected([
                new TackyUnary(TackyComplement.Operator, Const(2), Var(1)),
                new TackyBinary(TackyAddition.Operator, Var(1), Const(3), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(1 + 1);
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(1), Const(1), Var(1)),
                new TackyUnary(TackyComplement.Operator, Var(1), Var(2)),                
                new TackyReturn(Var(2))
            ])
        );
    } 
}