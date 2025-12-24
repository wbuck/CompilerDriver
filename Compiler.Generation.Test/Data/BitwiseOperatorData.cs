using Compiler.Generation.Instructions;
using Compiler.Generation.Registers;

namespace Compiler.Generation.Test.Data;

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
                AllocateStack(16),
                new Mov(Imm(3), Stack(-4)),
                new Bitwise(BitwiseAnd.Operator, Imm(5), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(1), Stack(-4)),
                new Bitwise(BitwiseOr.Operator, Imm(2), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(32),
                new Mov(Imm(80), Stack(-4)),
                new Bitwise(BitwiseRightShift.Operator, Imm(2), Stack(-4)),
                new Mov(Imm(7), Stack(-8)),
                new Bitwise(BitwiseLeftShift.Operator, Imm(1), Stack(-8)),
                new Mov(Imm(5), Stack(-12)),
                new Mov(Stack(-8), R10.Register),
                new Bitwise(BitwiseAnd.Operator, R10.Register, Stack(-12)),
                new Mov(Imm(1), Stack(-16)),
                new Mov(Stack(-12), R10.Register),
                new Bitwise(BitwiseXor.Operator, R10.Register, Stack(-16)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-20)),
                new Mov(Stack(-16), R10.Register),
                new Bitwise(BitwiseOr.Operator, R10.Register, Stack(-20)),
                new Mov(Stack(-20), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(33), Stack(-4)),
                new Bitwise(BitwiseRightShift.Operator, Imm(2), Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Bitwise(BitwiseLeftShift.Operator, Imm(1), Stack(-8)),
                new Mov(Stack(-8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(33), Stack(-4)),
                new Bitwise(BitwiseLeftShift.Operator, Imm(4), Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Bitwise(BitwiseRightShift.Operator, Imm(2), Stack(-8)),
                new Mov(Stack(-8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(4), Stack(-4)),
                new Binary(Instructions.Add.Operator, Imm(12), Stack(-4)),
                new Mov(Imm(40), Stack(-8)),
                new Mov(Stack(-4), Cx.Register),
                new Bitwise(BitwiseLeftShift.Operator, Cx.Register, Stack(-8)),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-12)),
                new Bitwise(BitwiseRightShift.Operator, Imm(1), Stack(-12)),
                new Mov(Stack(-12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(35), Stack(-4)),
                new Bitwise(BitwiseLeftShift.Operator, Imm(2), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(5), Stack(-4)),
                new Unary(Neg.Operator, Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Bitwise(BitwiseRightShift.Operator, Imm(30), Stack(-8)),
                new Mov(Stack(-8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(1000), Stack(-4)),
                new Bitwise(BitwiseRightShift.Operator, Imm(4), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(32),
                new Mov(Imm(2), Stack(-4)),
                new Mov(Stack(-4), R11.Register),
                new Binary(Mult.Operator, Imm(2), R11.Register),
                new Mov(R11.Register, Stack(-4)),
                new Mov(Imm(4), Stack(-8)),
                new Mov(Stack(-4), Cx.Register),
                new Bitwise(BitwiseLeftShift.Operator, Cx.Register, Stack(-8)),
                new Mov(Imm(1), Stack(-12)),
                new Binary(Instructions.Add.Operator, Imm(2), Stack(-12)),
                new Mov(Imm(100), Stack(-16)),
                new Mov(Stack(-12), Cx.Register),
                new Bitwise(BitwiseRightShift.Operator, Cx.Register, Stack(-16)),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-20)),
                new Mov(Stack(-16), R10.Register),
                new Binary(Instructions.Add.Operator, R10.Register, Stack(-20)),
                new Mov(Stack(-20), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(7), Stack(-4)),
                new Bitwise(BitwiseXor.Operator, Imm(1), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
            ])
        );
    }
}