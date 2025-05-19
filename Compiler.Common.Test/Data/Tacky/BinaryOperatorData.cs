using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

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
            Create([
                new TackyBinary(TackyAddition.Operator, Constant(1), Constant(2), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 6 / 3 / 2;
            }
            """,
            Create([
                new TackyBinary(TackyDivision.Operator, Constant(6), Constant(3), Variable(1)),
                new TackyBinary(TackyDivision.Operator, Variable(1), Constant(2), Variable(2)),
                new TackyReturn(Variable(2))
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
            Create([
                new TackyBinary(TackyMultiplication.Operator, Constant(5), Constant(4), Variable(1)),
                new TackyBinary(TackyDivision.Operator, Variable(1), Constant(2), Variable(2)),
                new TackyBinary(TackyAddition.Operator, Constant(2), Constant(1), Variable(3)),
                new TackyBinary(TackyRemainder.Operator, Constant(3), Variable(3), Variable(4)),
                new TackyBinary(TackySubtraction.Operator, Variable(2), Variable(4), Variable(5)),
                new TackyReturn(Variable(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (3 / 2 * 4) + (5 - 4 + 3);
            }
            """,
            Create([
                new TackyBinary(TackyDivision.Operator, Constant(3), Constant(2), Variable(1)),
                new TackyBinary(TackyMultiplication.Operator, Variable(1), Constant(4), Variable(2)),
                new TackyBinary(TackySubtraction.Operator, Constant(5), Constant(4), Variable(3)),
                new TackyBinary(TackyAddition.Operator, Variable(3), Constant(3), Variable(4)),
                new TackyBinary(TackyAddition.Operator, Variable(2), Variable(4), Variable(5)),
                new TackyReturn(Variable(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2 - 3;
            }
            """,
            Create([
                new TackyBinary(TackySubtraction.Operator, Constant(1), Constant(2), Variable(1)),
                new TackyBinary(TackySubtraction.Operator, Variable(1), Constant(3), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (-12) / 5;
            }
            """,
            Create([
                new TackyUnary(TackyNegate.Operator, Constant(12), Variable(1)),
                new TackyBinary(TackyDivision.Operator, Variable(1), Constant(5), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 / 2;
            }
            """,
            Create([
                new TackyBinary(TackyDivision.Operator, Constant(4), Constant(2), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 4 % 2;
            }
            """,
            Create([
                new TackyBinary(TackyRemainder.Operator, Constant(4), Constant(2), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * 3;
            }
            """,
            Create([
                new TackyBinary(TackyMultiplication.Operator, Constant(2), Constant(3), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 * (3 + 4);
            }
            """,
            Create([
                new TackyBinary(TackyAddition.Operator, Constant(3), Constant(4), Variable(1)),
                new TackyBinary(TackyMultiplication.Operator, Constant(2), Variable(1), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 + 3 * 4;
            }
            """,
            Create([                
                new TackyBinary(TackyMultiplication.Operator, Constant(3), Constant(4), Variable(1)),
                new TackyBinary(TackyAddition.Operator, Constant(2), Variable(1), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2- -1;
            }
            """,
            Create([                
                new TackyUnary(TackyNegate.Operator, Constant(1), Variable(1)),
                new TackyBinary(TackySubtraction.Operator, Constant(2), Variable(1), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 - 2;
            }
            """,
            Create([
                new TackyBinary(TackySubtraction.Operator, Constant(1), Constant(2), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 + 3;
            }
            """,
            Create([
                new TackyUnary(TackyComplement.Operator, Constant(2), Variable(1)),
                new TackyBinary(TackyAddition.Operator, Variable(1), Constant(3), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(1 + 1);
            }
            """,
            Create([
                new TackyBinary(TackyAddition.Operator, Constant(1), Constant(1), Variable(1)),
                new TackyUnary(TackyComplement.Operator, Variable(1), Variable(2)),                
                new TackyReturn(Variable(2))
            ])
        );
    } 
}