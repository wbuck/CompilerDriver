using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Tacky;

public class CompoundOperatorData : DataBase
{
    public CompoundOperatorData()
    {
        Add
        (
            """
            int main(void) {
                int a = 2;
                int b = 3 + a++;
                int c = 4 + ++b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(2), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("tmp.1")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyBinary(TackyAddition.Operator, Const(3), Var("tmp.1"), Var("tmp.2")),
                new TackyCopy(Var("tmp.2"), Var("b.1")),
                new TackyBinary(TackyAddition.Operator, Var("b.1"), Const(1), Var("b.1")),
                new TackyBinary(TackyAddition.Operator, Const(4), Var("b.1"), Var("tmp.3")),
                new TackyCopy(Var("tmp.3"), Var("c.2")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a <<= 0 || b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(11), Var("a.0")),
                new TackyCopy(Const(12), Var("b.1")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Var("b.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                new TackyJump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyBitwise(TackyLeftShift.Operator, Var("a.0"), Var("tmp.1"), Var("a.0")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a >>= 0 || b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(11), Var("a.0")),
                new TackyCopy(Const(12), Var("b.1")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Var("b.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                new TackyJump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyBitwise(TackyRightShift.Operator, Var("a.0"), Var("tmp.1"), Var("a.0")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a |= 0 || b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(11), Var("a.0")),
                new TackyCopy(Const(12), Var("b.1")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Var("b.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                new TackyJump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyBitwise(TackyBitwiseOr.Operator, Var("a.0"), Var("tmp.1"), Var("a.0")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a ^= 0 || b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(11), Var("a.0")),
                new TackyCopy(Const(12), Var("b.1")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Var("b.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                new TackyJump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyBitwise(TackyBitwiseXor.Operator, Var("a.0"), Var("tmp.1"), Var("a.0")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 11;
                int b = 12;
                a &= 0 || b;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(11), Var("a.0")),
                new TackyCopy(Const(12), Var("b.1")),
                new TackyJumpIfNotZero(Const(0), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyJumpIfNotZero(Var("b.1"), $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(0), Var("tmp.1")),
                new TackyJump($".{TackyConstants.OR_END_LABEL}2"),
                Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new TackyCopy(Const(1), Var("tmp.1")),
                Label($".{TackyConstants.OR_END_LABEL}2"),
                new TackyBitwise(TackyBitwiseAnd.Operator, Var("a.0"), Var("tmp.1"), Var("a.0")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 25;
                int b = 25;
                int c = 25;
                int d = 25;
                int e = 25;
                int f = 25;
                int g = 25;
                int h = 25;
                int i = 25;
                int j = 0;
                j = a &= b *= c |= d = e ^= f += g >>= h <<= i = 1;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(25), Var("a.0")),
                new TackyCopy(Const(25), Var("b.1")),
                new TackyCopy(Const(25), Var("c.2")),
                new TackyCopy(Const(25), Var("d.3")),
                new TackyCopy(Const(25), Var("e.4")),
                new TackyCopy(Const(25), Var("f.5")),
                new TackyCopy(Const(25), Var("g.6")),
                new TackyCopy(Const(25), Var("h.7")),
                new TackyCopy(Const(25), Var("i.8")),
                new TackyCopy(Const(0), Var("j.9")),
                new TackyCopy(Const(1), Var("i.8")),
                new TackyBitwise(TackyLeftShift.Operator, Var("h.7"), Var("i.8"), Var("h.7")),
                new TackyBitwise(TackyRightShift.Operator, Var("g.6"), Var("h.7"), Var("g.6")),
                new TackyBinary(TackyAddition.Operator, Var("f.5"), Var("g.6"), Var("f.5")),
                new TackyBitwise(TackyBitwiseXor.Operator, Var("e.4"), Var("f.5"), Var("e.4")),
                new TackyCopy(Var("e.4"), Var("d.3")),
                new TackyBitwise(TackyBitwiseOr.Operator, Var("c.2"), Var("d.3"), Var("c.2")),
                new TackyBinary(TackyMultiplication.Operator, Var("b.1"), Var("c.2"), Var("b.1")),
                new TackyBitwise(TackyBitwiseAnd.Operator, Var("a.0"), Var("b.1"), Var("a.0")),
                new TackyCopy(Var("a.0"), Var("j.9")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int c = -++(a);
                int d = !(b)--;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Const(2), Var("b.1")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyUnary(TackyNegate.Operator, Var("a.0"), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("c.2")),
                new TackyCopy(Var("b.1"), Var("tmp.2")),
                new TackyBinary(TackySubtraction.Operator, Var("b.1"), Const(1), Var("b.1")),
                new TackyUnary(TackyNot.Operator, Var("tmp.2"), Var("tmp.3")),
                new TackyCopy(Var("tmp.3"), Var("d.3")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int c = ++a;
                int d = --b;
                return 0;
            }
            """,
            GetExpected([
               new TackyCopy(Const(1), Var("a.0")),
               new TackyCopy(Const(2), Var("b.1")),
               new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
               new TackyCopy(Var("a.0"), Var("c.2")),
               new TackyBinary(TackySubtraction.Operator, Var("b.1"), Const(1), Var("b.1")),
               new TackyCopy(Var("b.1"), Var("d.3")),
               Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                int b = 2;
                int c = a++;
                int d = b--;
                return 0;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("a.0")),
                new TackyCopy(Const(2), Var("b.1")),
                new TackyCopy(Var("a.0"), Var("tmp.1")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyCopy(Var("tmp.1"), Var("c.2")),               
                new TackyCopy(Var("b.1"), Var("tmp.2")),
                new TackyBinary(TackySubtraction.Operator, Var("b.1"), Const(1), Var("b.1")),
                new TackyCopy(Var("tmp.2"), Var("d.3")),
                Ret(Const(0))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 15;
                int b = a ^ 5;
                return 1 | b;
            }
            """,
            GetExpected([
                new TackyCopy(Const(15), Var("a.0")),
                new TackyBitwise(TackyBitwiseXor.Operator, Var("a.0"), Const(5), Var("tmp.1")),
                new TackyCopy(Var("tmp.1"), Var("b.1")),
                new TackyBitwise(TackyBitwiseOr.Operator, Const(1), Var("b.1"), Var("tmp.2")),
                Ret(Var("tmp.2"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                int b = 0;
                a++;
                ++a;
                ++a;
                b--;
                --b;
                return a;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("a.0")),
                new TackyCopy(Const(0), Var("b.1")),
                new TackyCopy(Var("a.0"), Var("tmp.1")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyBinary(TackyAddition.Operator, Var("a.0"), Const(1), Var("a.0")),
                new TackyCopy(Var("b.1"), Var("tmp.2")),
                new TackyBinary(TackySubtraction.Operator, Var("b.1"), Const(1), Var("b.1")),
                new TackyBinary(TackySubtraction.Operator, Var("b.1"), Const(1), Var("b.1")),
                Ret(Var("a.0"))                
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_or = 1;
                to_or |= 30;
                return to_or;
            }
            """,
            GetExpected([
                new TackyCopy(Const(1), Var("to_or.0")),
                new TackyBitwise(TackyBitwiseOr.Operator, Var("to_or.0"), Const(30), Var("to_or.0")),
                Ret(Var("to_or.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_shiftl = 3;
                to_shiftl <<= 4;
                return to_shiftl;
            }
            """,
            GetExpected([
                new TackyCopy(Const(3), Var("to_shiftl.0")),
                new TackyBitwise(TackyLeftShift.Operator, Var("to_shiftl.0"), Const(4), Var("to_shiftl.0")),
                Ret(Var("to_shiftl.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_shiftr = 382574;
                to_shiftr >>= 4;
                return to_shiftr;
            }
            """,
            GetExpected([
                new TackyCopy(Const(382574), Var("to_shiftr.0")),
                new TackyBitwise(TackyRightShift.Operator, Var("to_shiftr.0"), Const(4), Var("to_shiftr.0")),
                Ret(Var("to_shiftr.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_xor = 7;
                to_xor ^= 5;
                return to_xor;
            }
            """,
            GetExpected([
                new TackyCopy(Const(7), Var("to_xor.0")),
                new TackyBitwise(TackyBitwiseXor.Operator, Var("to_xor.0"), Const(5), Var("to_xor.0")),
                Ret(Var("to_xor.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_divide = 8;
                to_divide /= 4;
                return to_divide;
            }
            """,
            GetExpected([
                new TackyCopy(Const(8), Var("to_divide.0")),
                new TackyBinary(TackyDivision.Operator, Var("to_divide.0"), Const(4), Var("to_divide.0")),
                Ret(Var("to_divide.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_subtract = 10;
                to_subtract -= 8;
                return to_subtract;
            }
            """,
            GetExpected([
                new TackyCopy(Const(10), Var("to_subtract.0")),
                new TackyBinary(TackySubtraction.Operator, Var("to_subtract.0"), Const(8), Var("to_subtract.0")),
                Ret(Var("to_subtract.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_mod = 5;
                to_mod %= 3;
                return to_mod;
            }
            """,
            GetExpected([
                new TackyCopy(Const(5), Var("to_mod.0")),
                new TackyBinary(TackyRemainder.Operator, Var("to_mod.0"), Const(3), Var("to_mod.0")),
                Ret(Var("to_mod.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_multiply = 4;
                to_multiply *= 3;
                return to_multiply;
            }
            """,
            GetExpected([
                new TackyCopy(Const(4), Var("to_multiply.0")),
                new TackyBinary(TackyMultiplication.Operator, Var("to_multiply.0"), Const(3), Var("to_multiply.0")),
                Ret(Var("to_multiply.0"))
            ])
        );
        Add
        (
            """
            int main(void) {
                int to_add = 0;
                to_add += 4;
                return to_add;
            }
            """,
            GetExpected([
                new TackyCopy(Const(0), Var("to_add.0")),
                new TackyBinary(TackyAddition.Operator, Var("to_add.0"), Const(4), Var("to_add.0")),
                Ret(Var("to_add.0"))
            ])
        );
    }
}