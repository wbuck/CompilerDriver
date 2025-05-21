using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.Assembly;

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
                AllocateStack(4),
                new Mov(Imm(1), Stack(4)),
                new Binary(Generation.Add.Operator, Imm(2), Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(6), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(3), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(2), R10.Register),
                new Div(R10.Register), 
                new Mov(Ax.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(20),
                new Mov(Imm(3), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(2), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Mov(Stack(8), R11.Register),
                new Binary(Mult.Operator, Imm(4), R11.Register),
                new Mov(R11.Register, Stack(8)),
                new Mov(Imm(5), Stack(12)),
                new Binary(Sub.Operator, Imm(4), Stack(12)),
                new Mov(Stack(12), R10.Register),
                new Mov(R10.Register, Stack(16)),
                new Binary(Generation.Add.Operator, Imm(3), Stack(16)),
                new Mov(Stack(8), R10.Register),
                new Mov(R10.Register, Stack(20)),
                new Mov(Stack(16), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(20)),
                new Mov(Stack(20), Ax.Register),
                Ret.Instruction
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
                AllocateStack(20),
                new Mov(Imm(5), Stack(4)),
                new Mov(Stack(4), R11.Register),
                new Binary(Mult.Operator, Imm(4), R11.Register),
                new Mov(R11.Register, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(2), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(8)),
                new Mov(Imm(2), Stack(12)),
                new Binary(Generation.Add.Operator, Imm(1), Stack(12)),
                new Mov(Imm(3), Ax.Register),
                Cdq.Instruction,
                new Div(Stack(12)),
                new Mov(Dx.Register, Stack(16)),
                new Mov(Stack(8), R10.Register),
                new Mov(R10.Register, Stack(20)),
                new Mov(Stack(16), R10.Register),
                new Binary(Sub.Operator, R10.Register, Stack(20)),
                new Mov(Stack(20), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(1), Stack(4)),
                new Binary(Sub.Operator, Imm(2), Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Binary(Sub.Operator, Imm(3), Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(12), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(5), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(2), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(2), R10.Register),
                new Div(R10.Register),
                new Mov(Dx.Register, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(2), Stack(4)),
                new Mov(Stack(4), R11.Register),
                new Binary(Mult.Operator, Imm(3), R11.Register),
                new Mov(R11.Register, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(3), Stack(4)),
                new Binary(Generation.Add.Operator, Imm(4), Stack(4)),
                new Mov(Imm(2), Stack(8)),
                new Mov(Stack(8), R11.Register),
                new Binary(Mult.Operator, Stack(4), R11.Register),
                new Mov(R11.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(8),
                new Mov(Imm(3), Stack(4)),
                new Mov(Stack(4), R11.Register),
                new Binary(Mult.Operator, Imm(4), R11.Register),
                new Mov(R11.Register, Stack(4)),
                new Mov(Imm(2), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(8),
                new Mov(Imm(1), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Imm(2), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Binary(Sub.Operator, R10.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(4),
                new Mov(Imm(1), Stack(4)),
                new Binary(Sub.Operator, Imm(2), Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(8),
                new Mov(Imm(2), Stack(4)),
                new Unary(Not.Operator, Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Binary(Generation.Add.Operator, Imm(3), Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(8),
                new Mov(Imm(1), Stack(4)),
                new Binary(Generation.Add.Operator, Imm(1), Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Unary(Not.Operator, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
            ])
        );
    }
}