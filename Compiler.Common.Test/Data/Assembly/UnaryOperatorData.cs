using Compiler.Common.Generation;

namespace Compiler.Common.Test.Data.Assembly;

public class UnaryOperatorData : DataBase
{
    public UnaryOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            GetExpected([
                AllocateStack(0),
                new Mov(Imm(42), Ax.Register),
                Ret.Instruction
            ])
        );  
        Add
        (
            """
            int main(void) {
                return -42;
            }
            """,
            GetExpected([
                    AllocateStack(4),
                    new Mov(Imm(42), Stack(4)),
                    new Unary(Neg.Operator, Stack(4)),
                    new Mov(Stack(4), Ax.Register),
                    Ret.Instruction
                ])
        );     
        Add
        (
            """
            int main(void) {
                return ~-42;
            }
            """,
            GetExpected([
                AllocateStack(8),
                new Mov(Imm(42), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Unary(Not.Operator, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
            ])
        );               
        Add
        (
            """
            int main(void) {
                return -(-42);
            }
            """,
            GetExpected([
                AllocateStack(8),
                new Mov(Imm(42), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Unary(Neg.Operator, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~-(-42);
            }
            """,
            GetExpected([
                AllocateStack(12),
                new Mov(Imm(42), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Unary(Neg.Operator, Stack(8)),
                new Mov(Stack(8), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Unary(Not.Operator, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
            ])
        );
    }    
}