using Compiler.Common.Generation;
using Compiler.Common.Tacky;

namespace Compiler.Common.Test.Data.Assembly;

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
                AllocateStack(20),
                new Mov(Imm(10), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(4)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(4)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}3"),
                new Mov(Imm(4), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}3"),
                new Mov(One, Stack(8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}4"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}3"),
                new Mov(Zero, Stack(8)),
                new Label($".{TackyConstants.AND_END_LABEL}4"),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Mov(Stack(8), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(12)),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}5"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}5"),
                new Mov(One, Stack(16)),
                new Jmp($".{TackyConstants.AND_END_LABEL}6"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}5"),
                new Mov(Zero, Stack(16)),
                new Label($".{TackyConstants.AND_END_LABEL}6"),
                new Mov(Stack(12), R10.Register),
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
                return 0 && (1 / 0);
            }
            """,
            GetExpected([
                AllocateStack(8),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Ax.Register),
                Cdq.Instruction,
                new Mov(Zero, R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                                                                                
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
                AllocateStack(8),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(Zero, R11.Register),
                new Cmp(Imm(5), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThanOrEqual.Code, Stack(4)),
                new Mov(One, R11.Register),
                new Cmp(Stack(4), R11.Register),
                new Mov(Zero, Stack(8)),
                new SetConditional(GreaterThan.Code, Stack(8)),
                new Mov(Zero, R11.Register),
                new Cmp(Stack(8), R11.Register),
                new Mov(Zero, Stack(12)),
                new SetConditional(LessThanOrEqual.Code, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(20),
                new Mov(Imm(2), Stack(4)),
                new Unary(Not.Operator, Stack(4)),
                new Mov(Imm(2), Stack(8)),
                new Unary(Neg.Operator, Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Mov(Stack(12), R11.Register),
                new Binary(Mult.Operator, Stack(8), R11.Register),
                new Mov(R11.Register, Stack(12)),
                new Mov(One, Stack(16)),
                new Binary(Generation.Add.Operator, Imm(5), Stack(16)),
                new Mov(Stack(12), R10.Register),
                new Cmp(R10.Register, Stack(16)),
                new Mov(Zero, Stack(20)),
                new SetConditional(Equal.Code, Stack(20)),
                new Mov(Stack(20), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(2), R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(One, R11.Register),
                new Cmp(Imm(3), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Imm(2), R11.Register),
                new Cmp(Stack(4), R11.Register),
                new Mov(Zero, Stack(8)),
                new SetConditional(NotEqual.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(One, R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(2), R11.Register),
                new Cmp(Imm(1), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThanOrEqual.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(16),
                new Mov(One, R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThanOrEqual.Code, Stack(4)),
                new Mov(Imm(4), Stack(8)),
                new Unary(Neg.Operator, Stack(8)),
                new Cmp(One, Stack(8)),
                new Mov(Zero, Stack(12)),
                new SetConditional(GreaterThanOrEqual.Code, Stack(12)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(16)),
                new Mov(Stack(12), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(16)),
                new Mov(Stack(16), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(Imm(2), R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThan.Code, Stack(4)),
                new Mov(One, R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(8)),
                new SetConditional(GreaterThan.Code, Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Mov(Stack(8), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(10), R11.Register),
                new Cmp(Imm(15), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThan.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction        
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
                AllocateStack(8),
                new Mov(One, Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Cmp(One, Stack(4)),
                new Mov(Zero, Stack(8)),
                new SetConditional(LessThanOrEqual.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(Imm(2), R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(LessThanOrEqual.Code, Stack(4)),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(8)),
                new SetConditional(LessThanOrEqual.Code, Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Mov(Stack(8), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(One, R11.Register),
                new Cmp(Imm(2), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(LessThan.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(2), R11.Register),
                new Cmp(One, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(LessThan.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(One, Ax.Register),
                Cdq.Instruction,
                new Mov(Zero, R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(One, Stack(8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}3"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(Zero, Stack(8)),
                new Label($".{TackyConstants.AND_END_LABEL}3"),
                new Cmp(Zero, Stack(8)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(12)),
                new Jmp($".{TackyConstants.OR_END_LABEL}4"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(12)),
                new Label($".{TackyConstants.OR_END_LABEL}4"),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(NotEqual.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(One, Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Mov(Imm(2), Stack(8)),
                new Unary(Neg.Operator, Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Cmp(R10.Register, Stack(8)),
                new Mov(Zero, Stack(12)),
                new SetConditional(NotEqual.Code, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(3), Stack(4)),
                new Unary(Neg.Operator, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new Mov(Zero, Stack(8)),
                new SetConditional(Equal.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(3), Stack(4)),
                new Binary(Sub.Operator, Imm(44), Stack(4)),
                new Cmp(Zero, Stack(4)),
                new Mov(Zero, Stack(8)),
                new SetConditional(Equal.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(4), Stack(4)),
                new Binary(Sub.Operator, Imm(4), Stack(4)),
                new Cmp(Zero, Stack(4)),
                new Mov(Zero, Stack(8)),
                new SetConditional(Equal.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(4),
                new Mov(Imm(5), R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Stack(4), Ax.Register),
                Ret.Instruction
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
                AllocateStack(20),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(4)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(4)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(8)),
                new Unary(Not.Operator, Stack(8)),
                new Mov(Imm(4), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(Imm(3), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(Zero, Stack(12)),
                new Jmp($".{TackyConstants.OR_END_LABEL}4"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(One, Stack(12)),
                new Label($".{TackyConstants.OR_END_LABEL}4"),
                new Mov(Stack(12), R10.Register),
                new Mov(R10.Register, Stack(16)),
                new Unary(Neg.Operator, Stack(16)),
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
                return 1 || (1 / 0);
            }
            """,
            GetExpected([
                AllocateStack(8),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Ax.Register),
                Cdq.Instruction,
                new Mov(Zero, R10.Register),
                new Div(R10.Register),
                new Mov(Ax.Register, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction                
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
                AllocateStack(20),
                new Mov(Imm(4), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(4)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(4)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(Imm(3), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(Zero, Stack(8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}4"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}3"),
                new Mov(One, Stack(8)),
                new Label($".{TackyConstants.OR_END_LABEL}4"),
                new Mov(Stack(4), R10.Register),
                new Mov(R10.Register, Stack(12)),
                new Mov(Stack(8), R10.Register),
                new Binary(Generation.Add.Operator, R10.Register, Stack(12)),
                new Mov(Imm(5), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new Mov(Imm(5), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new Mov(Zero, Stack(16)),
                new Jmp($".{TackyConstants.OR_END_LABEL}6"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}5"),
                new Mov(One, Stack(16)),
                new Label($".{TackyConstants.OR_END_LABEL}6"),
                new Mov(Stack(12), R10.Register),
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
                return (1 || 0) && 0;
            }
            """,
            GetExpected([
                AllocateStack(8),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new Mov(Zero, Stack(4)),
                new Jmp($".{TackyConstants.OR_END_LABEL}3"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}2"),
                new Mov(One, Stack(4)),
                new Label($".{TackyConstants.OR_END_LABEL}3"),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Jmp($".{TackyConstants.AND_END_LABEL}4"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Label($".{TackyConstants.AND_END_LABEL}4"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Zero, R11.Register),
                new Cmp(Imm(2), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(GreaterThanOrEqual.Code, Stack(4)),
                new Cmp(Imm(2), Stack(4)),
                new Mov(Zero, Stack(8)),
                new SetConditional(Equal.Code, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(2), R11.Register),
                new Cmp(Imm(2), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}2"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Label($".{TackyConstants.OR_END_LABEL}2"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(24),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Imm(2), Stack(8)),
                new Binary(Generation.Add.Operator, One, Stack(8)),
                new Mov(One, R11.Register),
                new Cmp(Stack(8), R11.Register),
                new Mov(Zero, Stack(12)),
                new SetConditional(GreaterThan.Code, Stack(12)),
                new Cmp(Imm(3), Stack(12)),
                new Mov(Zero, Stack(16)),
                new SetConditional(Equal.Code, Stack(16)),
                new Cmp(Zero, Stack(16)),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(One, Stack(20)),
                new Jmp($".{TackyConstants.AND_END_LABEL}2"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}1"),
                new Mov(Zero, Stack(20)),
                new Label($".{TackyConstants.AND_END_LABEL}2"),
                new Mov(Stack(20), R10.Register),
                new Mov(R10.Register, Stack(24)),
                new Binary(Generation.Add.Operator, One, Stack(24)),
                new Mov(Stack(24), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(One, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(Imm(2), R11.Register),
                new Cmp(Zero, R11.Register),
                new JmpConditional(Equal.Code, $".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(One, Stack(4)),
                new Jmp($".{TackyConstants.AND_END_LABEL}3"),
                new Label($".{TackyConstants.AND_WHEN_ZERO_LABEL}2"),
                new Mov(Zero, Stack(4)),
                new Label($".{TackyConstants.AND_END_LABEL}3"),
                new Cmp(Zero, Stack(4)),
                new JmpConditional(NotEqual.Code, $".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(Zero, Stack(8)),
                new Jmp($".{TackyConstants.OR_END_LABEL}4"),
                new Label($".{TackyConstants.OR_WHEN_NOT_ZERO_LABEL}1"),
                new Mov(One, Stack(8)),
                new Label($".{TackyConstants.OR_END_LABEL}4"),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(5), R11.Register),
                new Cmp(Imm(7), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(Equal.Code, Stack(4)),
                new Mov(Imm(5), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Bitwise(BitwiseAnd.Operator, R10.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(5), R11.Register),
                new Cmp(Imm(7), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(NotEqual.Code, Stack(4)),
                new Mov(Imm(5), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Bitwise(BitwiseOr.Operator, R10.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
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
                AllocateStack(12),
                new Mov(Imm(20), Stack(4)),                                
                new Bitwise(BitwiseRightShift.Operator, Imm(4), Stack(4)),
                new Mov(Imm(3), Stack(8)),
                new Bitwise(BitwiseLeftShift.Operator, Imm(1), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Cmp(R10.Register, Stack(8)),
                new Mov(Zero, Stack(12)),
                new SetConditional(LessThanOrEqual.Code, Stack(12)),
                new Mov(Stack(12), Ax.Register),
                Ret.Instruction
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
                AllocateStack(8),
                new Mov(Imm(5), R11.Register),
                new Cmp(Imm(7), R11.Register),
                new Mov(Zero, Stack(4)),
                new SetConditional(LessThan.Code, Stack(4)),
                new Mov(Imm(5), Stack(8)),
                new Mov(Stack(4), R10.Register),
                new Bitwise(BitwiseXor.Operator, R10.Register, Stack(8)),
                new Mov(Stack(8), Ax.Register),
                Ret.Instruction
            ])
        );
    }
}