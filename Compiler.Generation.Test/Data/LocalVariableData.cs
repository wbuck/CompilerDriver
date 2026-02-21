using Compiler.Generation.Instructions;
using Compiler.Generation.Registers;
using Compiler.Tacky.Helpers;

namespace Compiler.Generation.Test.Data;

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
                AllocateStack(16),
                new Mov(One, Stack(-4)),
                new Mov(Imm(2), Stack(-8)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-12)),
                new Mov(Stack(-8), R10.Register),
                new Binary(Instructions.Add.Operator, R10.Register, Stack(-12)),
                new Mov(Stack(-12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(48),
                new Mov(Imm(2147483646), Stack(-4)),
                new Mov(Zero, Stack(-8)),
                new Mov(Stack(-4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(6), R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(-12)),
                new Cmp(Zero, Stack(-8)),                
                new Mov(Zero, Stack(-16)),
                new SetConditional(Equal.Code, Stack(-16)),
                new Mov(Stack(-12), R10.Register),
                new Mov(R10.Register, Stack(-20)),
                new Mov(Stack(-16), R10.Register),
                new Binary(Instructions.Add.Operator, R10.Register, Stack(-20)),
                new Mov(Stack(-20), R10.Register),
                new Mov(R10.Register, Stack(-24)),
                new Mov(Stack(-24), R10.Register),
                new Mov(R10.Register, Stack(-28)),
                new Mov(Stack(-28), R11.Register),
                new Binary(Mult.Operator, Imm(2), R11.Register),
                new Mov(R11.Register, Stack(-28)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-32)),
                new Binary(Sub.Operator, Imm(1431655762), Stack(-32)),
                new Mov(Stack(-32), R10.Register),
                new Cmp(R10.Register, Stack(-28)),
                new Mov(Zero,  Stack(-36)),
                new SetConditional(Equal.Code, Stack(-36)),
                new Mov(Stack(-36), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(5), Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(2), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(0), Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Mov(Stack(-8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Imm(5), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(-4)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(-4)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Mov(Stack(-8), Ax.Register),
                Ret.Instruction
            ])
        );
        Add
        (
            """
            int main(void) {
            }
            """,
            GetExpected([
                AllocateStack(0),
            ])
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
                AllocateStack(16),
                new Mov(Imm(-2593), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Cdq.Instruction,
                new Mov(Imm(3), R10.Register),
                new Div(R10.Register),
                new Mov(Dx.Register, Stack(-8)),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-12)),
                new Unary(Neg.Operator, Stack(-12)),
                new Mov(Stack(-12), R10.Register),
                new Mov(R10.Register, Stack(-16)),
                new Mov(Stack(-16), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(3), Stack(-4)),
                new Mov(Imm(2), Stack(-8)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-12)),
                new Mov(Stack(-8), R10.Register),
                new Binary(Instructions.Add.Operator, R10.Register, Stack(-12)),
                new Mov(Stack(-12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(3), Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Binary(Instructions.Add.Operator, Imm(5), Stack(-8)),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-4)),                
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
                AllocateStack(16),
                new Mov(One, Stack(-4)),
                new Mov(Zero,  Stack(-8)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-8)),
                new Mov(Imm(3), Stack(-12)),
                new Mov(Stack(-12), R11.Register),
                new Binary(Mult.Operator, Stack(-8), R11.Register),
                new Mov(R11.Register, Stack(-12)),
                new Mov(Stack(-12), R10.Register),
                new Mov(R10.Register, Stack(-4)),
                new Mov(Stack(-4), R10.Register),
                new Mov(R10.Register, Stack(-16)),
                new Mov(Stack(-8), R10.Register),
                new Binary(Instructions.Add.Operator, R10.Register, Stack(-16)),
                new Mov(Stack(-16), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Zero, Stack(-4)),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(-4)),
                new Cmp(Zero, Stack(-4)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(-8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(-8)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
            ])
        );
        Add
        (
            """
            int main(void) {
                ;
            }
            """,
            GetExpected([
                AllocateStack(0),
            ])
        );
        Add
        (
            """
            int main(void) {
                ;
                return 0;
            }
            """,
            GetExpected([
                AllocateStack(0),
                new Mov(Zero, Ax.Register),
                Ret.Instruction
            ])
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
                AllocateStack(16),
                new Mov(Imm(2), Stack(-4)),
                new Mov(Stack(-4), Ax.Register),               
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(0), Stack(-4)),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Imm(5), Stack(-4)),
                new Cmp(Zero, Stack(-4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(-8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(-8)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Zero, Stack(-4)),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(-4)),
                new Cmp(Zero, Stack(-4)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(-8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(-8)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Imm(2), Stack(-4)),
                new Binary(Instructions.Add.Operator, Imm(2), Stack(-4)),
                new Mov(Zero, Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(One,  Stack(-4)),
                new Mov(Imm(2), Stack(-8)),
                new Mov(Imm(4), Stack(-8)),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Cmp(Zero, Stack(-4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(-8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(-8)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(-8), R10.Register),
                new Mov(R10.Register, Stack(-4)),
                new Mov(Stack(-4), Ax.Register),
                Ret.Instruction
            ])
        );
    }
}