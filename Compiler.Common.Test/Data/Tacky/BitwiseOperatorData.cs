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
            GetExpected([
                new TackyBitwise(TackyBitwiseAnd.Operator, Const(3), Const(5), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            GetExpected([
                new TackyBitwise(TackyBitwiseOr.Operator, Const(1), Const(2), Var(1)),
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 80 >> 2 | 1 ^ 5 & 7 << 1;
            }
            """,
            GetExpected([
                new TackyBitwise(TackyRightShift.Operator, Const(80), Const(2), Var(1)),
                new TackyBitwise(TackyLeftShift.Operator, Const(7), Const(1), Var(2)),
                new TackyBitwise(TackyBitwiseAnd.Operator, Const(5), Var(2), Var(3)),
                new TackyBitwise(TackyBitwiseXor.Operator, Const(1), Var(3), Var(4)),
                new TackyBitwise(TackyBitwiseOr.Operator, Var(1), Var(4), Var(5)),
                new TackyReturn(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 >> 2 << 1;
            }
            """,
            GetExpected([
                new TackyBitwise(TackyRightShift.Operator, Const(33), Const(2), Var(1)),
                new TackyBitwise(TackyLeftShift.Operator, Var(1), Const(1), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 << 4 >> 2;
            }
            """,
            GetExpected([
                new TackyBitwise(TackyLeftShift.Operator, Const(33), Const(4), Var(1)),
                new TackyBitwise(TackyRightShift.Operator, Var(1), Const(2), Var(2)),
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 40 << 4 + 12 >> 1;
            }
            """,
            GetExpected([
                new TackyBinary(TackyAddition.Operator, Const(4), Const(12), Var(1)),
                new TackyBitwise(TackyLeftShift.Operator, Const(40), Var(1), Var(2)),
                new TackyBitwise(TackyRightShift.Operator, Var(2), Const(1), Var(3)),
                new TackyReturn(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            GetExpected([                
                new TackyBitwise(TackyLeftShift.Operator, Const(35), Const(2), Var(1)),                
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return -5 >> 30;
            }
            """,
            GetExpected([         
                new TackyUnary(TackyNegate.Operator, Const(5), Var(1)),
                new TackyBitwise(TackyRightShift.Operator, Var(1), Const(30), Var(2)),                
                new TackyReturn(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            GetExpected([                         
                new TackyBitwise(TackyRightShift.Operator, Const(1000), Const(4), Var(1)),                
                new TackyReturn(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 << (2 * 2)) + (100 >> (1 + 2));
            }
            """,
            GetExpected([                         
                new TackyBinary(TackyMultiplication.Operator, Const(2), Const(2), Var(1)),
                new TackyBitwise(TackyLeftShift.Operator, Const(4), Var(1), Var(2)),
                new TackyBinary(TackyAddition.Operator, Const(1), Const(2), Var(3)),
                new TackyBitwise(TackyRightShift.Operator, Const(100), Var(3), Var(4)),
                new TackyBinary(TackyAddition.Operator, Var(2), Var(4), Var(5)),
                new TackyReturn(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 7 ^ 1;
            }
            """,
            GetExpected([                         
                new TackyBitwise(TackyBitwiseXor.Operator, Const(7), Const(1), Var(1)),                
                new TackyReturn(Var(1))
            ])
        );
    }
}