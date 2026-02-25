using Compiler.Generation.Instructions;
using Compiler.Generation.Registers;

namespace Compiler.Generation.Test.Data;

public class FunctionData : DataBase
{
    public FunctionData()
    {
        Add
        (
            """
            int putchar(int c);
            
            int foo(int a, int b, int c, int d, int e, int f, int g, int h) {
                putchar(h);
                return a + g;
            }
            
            int main(void) {
                return foo(1, 2, 3, 4, 5, 6, 7, 65);
            }
            """,
            GetExpected([
                new Function("foo", true, [
                    AllocateStack(48),
                    Mov(Di.Register, Stack(-4)),
                    Mov(Si.Register, Stack(-8)),
                    Mov(Dx.Register, Stack(-12)),
                    Mov(Cx.Register, Stack(-16)),
                    Mov(R8.Register, Stack(-20)),
                    Mov(R9.Register, Stack(-24)),
                    Mov(Stack(16), R10.Register),
                    Mov(R10.Register, Stack(-28)),
                    Mov(Stack(24), R10.Register),
                    Mov(R10.Register, Stack(-32)),
                    Mov(Stack(-32), Di.Register),
                    new Call("putchar"),
                    new Mov(Ax.Register, Stack(-36)),
                    new Mov(Stack(-4), R10.Register),
                    new Mov(R10.Register, Stack(-40)),
                    new Mov(Stack(-28), R10.Register),
                    new Binary(Instructions.Add.Operator, R10.Register, Stack(-40)),
                    new Mov(Stack(-40), Ax.Register),
                    Ret.Instruction,
                    new Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(16),
                    MovConstantToDi(1),
                    MovConstantToSi(2),
                    MovConstantToDx(3),
                    MovConstantToCx(4),
                    MovConstantToR8(5),
                    MovConstantToR9(6),
                    new Push(Imm(65)),
                    new Push(Imm(7)),
                    new Call("foo"),
                    new DeallocateStack(16),
                    MovAxToStack(-4),
                    MovStackToAx(-4),
                    Ret.Instruction,
                    new Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
        Add
        (
            """
            int sub(int a, int b) {
                return a - b;
            }
            
            int main(void) {
                int sum = sub(1 + 2, 1);
                return sum;
            }
            """,
            GetExpected([
                new Function("sub", true, [
                    AllocateStack(16),
                    MovDiToStack(-4),
                    MovSiToStack(-8),
                    MovStackToR10(-4),
                    MovR10ToStack(-12),
                    MovStackToR10(-8),
                    new Binary(Sub.Operator, R10.Register, Stack(-12)),
                    MovStackToAx(-12),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(16),
                    MovConstantToStack(1, -4),
                    new Binary(Instructions.Add.Operator, Imm(2), Stack(-4)),
                    Mov(Stack(-4), Di.Register),
                    MovConstantToSi(1),
                    new Call("sub"),
                    MovAxToStack(-8),
                    MovStackToR10(-8),
                    MovR10ToStack(-12),
                    MovStackToAx(-12),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
        Add
        (
            """
            int x(int a, int b, int c, int d, int e, int f) {
                return a + f;
            }
            
            int main(void) {
                int a = 4;
                return x(1, 2, 3, 4, 5, 24 / a);
            }
            """,
            GetExpected([
                new Function("x", true, [
                    AllocateStack(32),
                    MovDiToStack(-4),
                    MovSiToStack(-8),
                    MovDxToStack(-12),
                    MovCxToStack(-16),
                    MovR8ToStack(-20),
                    MovR9ToStack(-24),
                    MovStackToR10(-4),
                    MovR10ToStack(-28),
                    MovStackToR10(-24),
                    new Binary(Instructions.Add.Operator, R10.Register, Stack(-28)),
                    MovStackToAx(-28),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(16),
                    MovConstantToStack(4, -4),
                    new Mov(Imm(24), Ax.Register),
                    Cdq.Instruction,
                    new Div(Stack(-4)),
                    MovAxToStack(-8),
                    MovConstantToDi(1),
                    MovConstantToSi(2),
                    MovConstantToDx(3),
                    MovConstantToCx(4),
                    MovConstantToR8(5),
                    Mov(Stack(-8), R9.Register),
                    new Call("x"),
                    MovAxToStack(-12),
                    MovStackToAx(-12),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
        Add
        (
            """
            int lots_of_args(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o) {
                return l + o;
            }
            
            int main(void) {
                int ret = 0;
                for (int i = 0; i < 10000000; i = i + 1) {
                    ret = lots_of_args(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, ret, 13, 14, 15);
                }
                return ret == 150000000;
            }
            """,
            GetExpected([
                new Function("lots_of_args", true, [
                    AllocateStack(64),                    
                    Mov(Di.Register, Stack(-4)),
                    Mov(Si.Register, Stack(-8)),
                    Mov(Dx.Register, Stack(-12)),
                    Mov(Cx.Register, Stack(-16)),
                    Mov(R8.Register, Stack(-20)),
                    Mov(R9.Register, Stack(-24)),
                    Mov(Stack(16), R10.Register),
                    Mov(R10.Register, Stack(-28)),
                    Mov(Stack(24), R10.Register),
                    Mov(R10.Register, Stack(-32)),
                    Mov(Stack(32), R10.Register),
                    Mov(R10.Register, Stack(-36)),
                    Mov(Stack(40), R10.Register),
                    Mov(R10.Register, Stack(-40)),
                    Mov(Stack(48), R10.Register),
                    Mov(R10.Register, Stack(-44)),
                    Mov(Stack(56), R10.Register),
                    Mov(R10.Register, Stack(-48)),
                    Mov(Stack(64), R10.Register),
                    Mov(R10.Register, Stack(-52)),
                    Mov(Stack(72), R10.Register),
                    Mov(R10.Register, Stack(-56)),
                    Mov(Stack(80), R10.Register),
                    Mov(R10.Register, Stack(-60)),
                    Mov(Stack(-48), R10.Register),                 
                    Mov(R10.Register, Stack(-64)),
                    Mov(Stack(-60), R10.Register),
                    new Binary(Instructions.Add.Operator, R10.Register, Stack(-64)),
                    Mov(Stack(-64), Ax.Register),
                    Ret.Instruction,                                        
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(32),
                    Mov(Zero, Stack(-4)),
                    Mov(Zero, Stack(-8)),
                    new Label(".begin.for1"),
                    new Cmp(Imm(10_000_000), Stack(-8)),
                    Mov(Zero, Stack(-12)),                    
                    new SetConditional(LessThan.Code, Stack(-12)),
                    new Cmp(Zero, Stack(-12)),
                    new JmpConditional(Equal.Code, $".break.for1"),
                    AllocateStack(8),
                    Mov(One, Di.Register),
                    Mov(Imm(2), Si.Register),
                    Mov(Imm(3), Dx.Register),
                    Mov(Imm(4), Cx.Register),
                    Mov(Imm(5), R8.Register),
                    Mov(Imm(6), R9.Register),
                    new Push(Imm(15)),
                    new Push(Imm(14)),
                    new Push(Imm(13)),
                    Mov(Stack(-4), Ax.Register),
                    new Push(Ax.Register),
                    new Push(Imm(11)),
                    new Push(Imm(10)),
                    new Push(Imm(9)),
                    new Push(Imm(8)),
                    new Push(Imm(7)),
                    new Call("lots_of_args"),
                    new DeallocateStack(80),
                    Mov(Ax.Register, Stack(-16)),
                    Mov(Stack(-16), R10.Register),
                    Mov(R10.Register, Stack(-4)),
                    new Label(".continue.for1"),
                    Mov(Stack(-8), R10.Register),
                    Mov(R10.Register, Stack(-20)),
                    new Binary(Instructions.Add.Operator, One, Stack(-20)),
                    Mov(Stack(-20), R10.Register),
                    Mov(R10.Register, Stack(-8)),
                    new Jmp($".begin.for1"),
                    new Label(".break.for1"),
                    new Cmp(Imm(150_000_000), Stack(-4)),                    
                    Mov(Zero, Stack(-24)),
                    new SetConditional(Equal.Code, Stack(-24)),
                    Mov(Stack(-24), Ax.Register),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
        Add
        (
            """
            int foo(int a, int b, int c, int d, int e, int f, int g, int h) {
                return a + g;
            }
            
            int main(void) {
                return foo(1, 2, 3, 4, 5, 6, 7, 65);
            }
            """,
            GetExpected([
                new Function("foo", true, [
                    AllocateStack(48),
                    Mov(Di.Register, Stack(-4)),
                    Mov(Si.Register, Stack(-8)),
                    Mov(Dx.Register, Stack(-12)),
                    Mov(Cx.Register, Stack(-16)),
                    Mov(R8.Register, Stack(-20)),
                    Mov(R9.Register, Stack(-24)),
                    Mov(Stack(16), R10.Register),
                    Mov(R10.Register, Stack(-28)),
                    Mov(Stack(24), R10.Register),
                    Mov(R10.Register, Stack(-32)),
                    Mov(Stack(-4), R10.Register),
                    Mov(R10.Register, Stack(-36)),
                    Mov(Stack(-28), R10.Register),
                    new Binary(Instructions.Add.Operator, R10.Register, Stack(-36)),
                    Mov(Stack(-36), Ax.Register),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(16),
                    Mov(One, Di.Register),
                    Mov(Imm(2), Si.Register),
                    Mov(Imm(3), Dx.Register),
                    Mov(Imm(4), Cx.Register),
                    Mov(Imm(5), R8.Register),
                    Mov(Imm(6), R9.Register),
                    new Push(Imm(65)),
                    new Push(Imm(7)),
                    Call("foo"),
                    new DeallocateStack(16),
                    Mov(Ax.Register, Stack(-4)),
                    Mov(Stack(-4), Ax.Register),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
        Add
        (
            """
            int twice(int x){
                return 2 * x;
            }
            
            int main(void) {
                return twice(3);
            }
            """,
            GetExpected([
                new Function("twice", true, [
                    AllocateStack(16),
                    Mov(Di.Register, Stack(-4)),
                    Mov(Imm(2), Stack(-8)),
                    Mov(Stack(-8), R11.Register),
                    new Binary(Mult.Operator, Stack(-4), R11.Register),
                    Mov(R11.Register, Stack(-8)),
                    Mov(Stack(-8), Ax.Register),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ]),
                new Function("main", true, [
                    AllocateStack(16),
                    Mov(Imm(3), Di.Register),
                    Call("twice"),
                    Mov(Ax.Register, Stack(-4)),
                    Mov(Stack(-4), Ax.Register),
                    Ret.Instruction,
                    Mov(Zero, Ax.Register),
                    Ret.Instruction
                ])
            ])
        );
    }
}