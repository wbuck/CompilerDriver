using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class BitwiseOperatorData : DataBase
{
    public BitwiseOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 3 & 5;
            }
            """,
            Create([
                new TackyBitwise(TackyAnd.Operator, Constant(3), Constant(5), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            Create([
                new TackyBitwise(TackyOr.Operator, Constant(1), Constant(2), Variable(1)),
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 80 >> 2 | 1 ^ 5 & 7 << 1;
            }
            """,
            Create([
                new TackyBitwise(TackyRightShift.Operator, Constant(80), Constant(2), Variable(1)),
                new TackyBitwise(TackyLeftShift.Operator, Constant(7), Constant(1), Variable(2)),
                new TackyBitwise(TackyAnd.Operator, Constant(5), Variable(2), Variable(3)),
                new TackyBitwise(TackyXor.Operator, Constant(1), Variable(3), Variable(4)),
                new TackyBitwise(TackyOr.Operator, Variable(1), Variable(4), Variable(5)),
                new TackyReturn(Variable(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 >> 2 << 1;
            }
            """,
            Create([
                new TackyBitwise(TackyRightShift.Operator, Constant(33), Constant(2), Variable(1)),
                new TackyBitwise(TackyLeftShift.Operator, Variable(1), Constant(1), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 << 4 >> 2;
            }
            """,
            Create([
                new TackyBitwise(TackyLeftShift.Operator, Constant(33), Constant(4), Variable(1)),
                new TackyBitwise(TackyRightShift.Operator, Variable(1), Constant(2), Variable(2)),
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 40 << 4 + 12 >> 1;
            }
            """,
            Create([
                new TackyBinary(TackyAddition.Operator, Constant(4), Constant(12), Variable(1)),
                new TackyBitwise(TackyLeftShift.Operator, Constant(40), Variable(1), Variable(2)),
                new TackyBitwise(TackyRightShift.Operator, Variable(2), Constant(1), Variable(3)),
                new TackyReturn(Variable(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            Create([                
                new TackyBitwise(TackyLeftShift.Operator, Constant(35), Constant(2), Variable(1)),                
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return -5 >> 30;
            }
            """,
            Create([         
                new TackyUnary(TackyNegate.Operator, Constant(5), Variable(1)),
                new TackyBitwise(TackyRightShift.Operator, Variable(1), Constant(30), Variable(2)),                
                new TackyReturn(Variable(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            Create([                         
                new TackyBitwise(TackyRightShift.Operator, Constant(1000), Constant(4), Variable(1)),                
                new TackyReturn(Variable(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 << (2 * 2)) + (100 >> (1 + 2));
            }
            """,
            Create([                         
                new TackyBinary(TackyMultiplication.Operator, Constant(2), Constant(2), Variable(1)),
                new TackyBitwise(TackyLeftShift.Operator, Constant(4), Variable(1), Variable(2)),
                new TackyBinary(TackyAddition.Operator, Constant(1), Constant(2), Variable(3)),
                new TackyBitwise(TackyRightShift.Operator, Constant(100), Variable(3), Variable(4)),
                new TackyBinary(TackyAddition.Operator, Variable(2), Variable(4), Variable(5)),
                new TackyReturn(Variable(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 7 ^ 1;
            }
            """,
            Create([                         
                new TackyBitwise(TackyXor.Operator, Constant(7), Constant(1), Variable(1)),                
                new TackyReturn(Variable(1))
            ])
        );
    }
}