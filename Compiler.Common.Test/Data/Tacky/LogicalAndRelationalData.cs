using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class LogicalAndRelationalData : DataBase
{
    public LogicalAndRelationalData()
    {
        Add
        (
            """
            int main(void) {
                return (10 && 0) + (0 && 4) + (0 && 0);
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(10), ".AND_WHEN_ZERO_L1"),
                new TackyJumpIfZero(Const(0), ".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(1), Var(1)),
                Jump(".AND_END_L2"),
                Label(".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(0), Var(1)),
                Label(".AND_END_L2"),
                new TackyJumpIfZero(Const(0), ".AND_WHEN_ZERO_L3"),
                new TackyJumpIfZero(Const(4), ".AND_WHEN_ZERO_L3"),
                new TackyCopy(Const(1), Var(2)),
                Jump(".AND_END_L4"),
                Label(".AND_WHEN_ZERO_L3"),
                new TackyCopy(Const(0), Var(2)),
                Label(".AND_END_L4"),
                new TackyBinary(TackyAddition.Operator, Var(1), Var(2), Var(3)),
                new TackyJumpIfZero(Const(0), ".AND_WHEN_ZERO_L5"),
                new TackyJumpIfZero(Const(0), ".AND_WHEN_ZERO_L5"),
                new TackyCopy(Const(1), Var(4)),
                Jump(".AND_END_L6"),
                Label(".AND_WHEN_ZERO_L5"),
                new TackyCopy(Const(0), Var(4)),
                Label(".AND_END_L6"),
                new TackyBinary(TackyAddition.Operator, Var(3), Var(4), Var(5)),
                Ret(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 && (1 / 0);
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(0), ".AND_WHEN_ZERO_L1"),
                new TackyBinary(TackyDivision.Operator, Const(1), Const(0), Var(1)),
                new TackyJumpIfZero(Var(1), ".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(1), Var(2)),
                Jump(".AND_END_L2"),
                Label(".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(0), Var(2)),
                Label(".AND_END_L2"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 && -1;
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(1), ".AND_WHEN_ZERO_L1"),
                new TackyUnary(TackyNegate.Operator, Const(1), Var(1)),
                new TackyJumpIfZero(Var(1), ".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(1), Var(2)),
                Jump(".AND_END_L2"),
                Label(".AND_WHEN_ZERO_L1"),
                new TackyCopy(Const(0), Var(2)),
                Label(".AND_END_L2"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 >= 0 > 1 <= 0;
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThanOrEqual.Operator, Const(5), Const(0), Var(1)),
                new TackyBinary(TackyGreaterThan.Operator, Var(1), Const(1), Var(2)),
                new TackyBinary(TackyLessThanOrEqual.Operator, Var(2), Const(0), Var(3)),
                Ret(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 * -2 == 1 + 5;
            }
            """,
            GetExpected([
                new TackyUnary(TackyComplement.Operator, Const(2), Var(1)),
                new TackyUnary(TackyNegate.Operator, Const(2), Var(2)),
                new TackyBinary(TackyMultiplication.Operator, Var(1), Var(2), Var(3)),
                new TackyBinary(TackyAddition.Operator, Const(1), Const(5), Var(4)),
                new TackyBinary(TackyEqual.Operator, Var(3), Var(4), Var(5)),
                Ret(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(1), Const(2), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 3 == 1 != 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(3), Const(1), Var(1)),
                new TackyBinary(TackyNotEqual.Operator, Var(1), Const(2), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 1;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(1), Const(1), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 >= 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThanOrEqual.Operator, Const(1), Const(2), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 >= 1) + (1 >= -4);
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThanOrEqual.Operator, Const(1), Const(1), Var(1)),
                new TackyUnary(TackyNegate.Operator, Const(4), Var(2)),
                new TackyBinary(TackyGreaterThanOrEqual.Operator, Const(1), Var(2), Var(3)),
                new TackyBinary(TackyAddition.Operator, Var(1), Var(3), Var(4)),
                Ret(Var(4))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 > 2) + (1 > 1);
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThan.Operator, Const(1), Const(2), Var(1)),
                new TackyBinary(TackyGreaterThan.Operator, Const(1), Const(1), Var(2)),
                new TackyBinary(TackyAddition.Operator, Var(1), Var(2), Var(3)),
                Ret(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 15 > 10;
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThan.Operator, Const(15), Const(10), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 <= -1;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(1), Var(1)),
                new TackyBinary(TackyLessThanOrEqual.Operator, Const(1), Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 <= 2) + (0 <= 0);
            }
            """,
            GetExpected([
                new TackyBinary(TackyLessThanOrEqual.Operator, Const(0), Const(2), Var(1)),
                new TackyBinary(TackyLessThanOrEqual.Operator, Const(0), Const(0), Var(2)),
                new TackyBinary(TackyAddition.Operator, Var(1), Var(2), Var(3)),
                Ret(Var(3))
            ])
        );
        Add
                  (
                      """
                      int main(void) {
                          return 2 < 1;
                      }
                      """,
                      GetExpected([
                          new TackyBinary(TackyLessThan.Operator, Const(2), Const(1), Var(1)),
                          Ret(Var(1))
                      ])
                  );
        Add
        (
            """
            int main(void) {
                return 1 < 2;
            }
            """,
            GetExpected([
                new TackyBinary(TackyLessThan.Operator, Const(1), Const(2), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 || 0 && (1 / 0);
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyBinary(TackyDivision.Operator, Const(1), Const(0), Var(1)) ,
                new TackyJumpIfZero(Var(1), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(1), Var(2)),
                Jump($".{TackyConstants.AND_END_LABEL}3"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(0), Var(2)),
                Label($".{TackyConstants.AND_END_LABEL}3"),
                new TackyJumpIfNotZero(Var(2), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(3)),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(3)),
                Label($".{TackyConstants.OR_END_LABEL}4"),
                Ret(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 != 0;
            }
            """,
            GetExpected([
                new TackyBinary(TackyNotEqual.Operator, Const(0), Const(0), Var(1)),
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return -1 != -2;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(1), Var(1)),
                new TackyUnary(TackyNegate.Operator, Const(2), Var(2)),
                new TackyBinary(TackyNotEqual.Operator, Var(1), Var(2), Var(3)),
                Ret(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return !-3;
            }
            """,
            GetExpected([
                new TackyUnary(TackyNegate.Operator, Const(3), Var(1)),
                new TackyUnary(TackyNot.Operator, Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(3 - 44);
            }
            """,
            GetExpected([
                new TackyBinary(TackySubtraction.Operator, Const(3), Const(44), Var(1)),
                new TackyUnary(TackyNot.Operator, Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(4-4);
            }
            """,
            GetExpected([
                new TackyBinary(TackySubtraction.Operator, Const(4), Const(4), Var(1)),
                new TackyUnary(TackyNot.Operator, Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return !0;
            }
            """,            
            GetExpected([                
                new TackyUnary(TackyNot.Operator, Const(0), Var(1)),                
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return !5;
            }
            """,
            GetExpected([                
                new TackyUnary(TackyNot.Operator, Const(5), Var(1)),                
                Ret(Var(1))
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(0 && 1) - -(4 || 3);
            }
            """,
            GetExpected([
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyJumpIfZero(Const(1), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Jump($".{TackyConstants.AND_END_LABEL}2"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Label($".{TackyConstants.AND_END_LABEL}2"),
                new TackyUnary(TackyComplement.Operator, Var(1), Var(2)),
                new TackyJumpIfNotZero(Const(4), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyJumpIfNotZero(Const(3), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(0), Var(3)),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(1), Var(3)),
                Label($".{TackyConstants.OR_END_LABEL}4"),
                new TackyUnary(TackyNegate.Operator, Var(3), Var(4)),
                new TackyBinary(TackySubtraction.Operator, Var(2), Var(4), Var(5)),
                Ret(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 || (1 / 0);
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyBinary(TackyDivision.Operator, Const(1), Const(0), Var(1)),
                new TackyJumpIfNotZero(Var(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(2)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(2)),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 || 0) + (0 || 3) + (5 || 5);
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(4), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(1)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(1)),
                Label($".{TackyConstants.OR_END_LABEL}2"),                
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyJumpIfNotZero(Const(3), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(0), Var(2)),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new TackyCopy(Const(1), Var(2)),
                Label($".{TackyConstants.OR_END_LABEL}4"),                
                new TackyBinary(TackyAddition.Operator, Var(1), Var(2), Var(3)),                
                new TackyJumpIfNotZero(Const(5), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new TackyJumpIfNotZero(Const(5), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new TackyCopy(Const(0), Var(4)),
                Jump($".{TackyConstants.OR_END_LABEL}6"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new TackyCopy(Const(1), Var(4)),
                Label($".{TackyConstants.OR_END_LABEL}6"),                
                new TackyBinary(TackyAddition.Operator, Var(3), Var(4), Var(5)),
                Ret(Var(5))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 || 0) && 0;
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new TackyCopy(Const(0), Var(1)),
                Jump($".{TackyConstants.OR_END_LABEL}3"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new TackyCopy(Const(1), Var(1)),
                Label($".{TackyConstants.OR_END_LABEL}3"),
                new TackyJumpIfZero(Var(1), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(2)),
                Jump($".{TackyConstants.AND_END_LABEL}4"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(2)),
                Label($".{TackyConstants.AND_END_LABEL}4"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 >= 0;
            }
            """,
            GetExpected([
                new TackyBinary(TackyGreaterThanOrEqual.Operator, Const(2), Const(0), Var(1)),
                new TackyBinary(TackyEqual.Operator, Const(2), Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 || 0;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(2), Const(2), Var(1)),
                new TackyJumpIfNotZero(Var(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(2)),
                Jump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(2)),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 == 0 && 3 == 2 + 1 > 1) + 1;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(0), Const(0), Var(1)),
                new TackyJumpIfZero(Var(1), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyBinary(TackyAddition.Operator, Const(2), Const(1), Var(2)),
                new TackyBinary(TackyGreaterThan.Operator, Var(2), Const(1), Var(3)),
                new TackyBinary(TackyEqual.Operator, Const(3), Var(3), Var(4)),
                new TackyJumpIfZero(Var(4), $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(5)),
                Jump($".{TackyConstants.AND_END_LABEL}2"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(5)),
                Label($".{TackyConstants.AND_END_LABEL}2"),
                new TackyBinary(TackyAddition.Operator, Var(5), Const(1), Var(6)),
                Ret(Var(6))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 || 0 && 2;
            }
            """,
            GetExpected([
                new TackyJumpIfNotZero(Const(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfZero(Const(0), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyJumpIfZero(Const(2), $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(1), Var(1)),
                Jump($".{TackyConstants.AND_END_LABEL}3"),
                Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new TackyCopy(Const(0), Var(1)),
                Label($".{TackyConstants.AND_END_LABEL}3"),
                new TackyJumpIfNotZero(Var(1), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var(2)),
                Jump($".{TackyConstants.OR_END_LABEL}4"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var(2)),
                Label($".{TackyConstants.OR_END_LABEL}4"),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 & 7 == 5;
            }
            """,
            GetExpected([
                new TackyBinary(TackyEqual.Operator, Const(7), Const(5), Var(1)),
                new TackyBitwise(TackyBitwiseAnd.Operator, Const(5), Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 | 7 != 5;
            }
            """,
            GetExpected([
                new TackyBinary(TackyNotEqual.Operator, Const(7), Const(5), Var(1)),
                new TackyBitwise(TackyBitwiseOr.Operator, Const(5), Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 20 >> 4 <= 3 << 1;
            }
            """,
            GetExpected([
                new TackyBitwise(TackyRightShift.Operator, Const(20), Const(4), Var(1)),
                new TackyBitwise(TackyLeftShift.Operator, Const(3), Const(1), Var(2)),
                new TackyBinary(TackyLessThanOrEqual.Operator, Var(1), Var(2), Var(3)),
                Ret(Var(3))
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 ^ 7 < 5;
            }
            """,
            GetExpected([
                new TackyBinary(TackyLessThan.Operator, Const(7), Const(5), Var(1)),
                new TackyBitwise(TackyBitwiseXor.Operator, Const(5), Var(1), Var(2)),
                Ret(Var(2))
            ])
        );
    }
}