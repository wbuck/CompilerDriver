using Compiler.Generation.Instructions;
using Compiler.Generation.Registers;

namespace Compiler.Generation.Test.Data;

public class DataBase : TheoryData<string, Program>
{
    protected static Program GetExpected(List<IInstruction> instructions)
    {
        instructions.Add(new Mov(Zero, Ax.Register));
        instructions.Add(Ret.Instruction);
        return new Program([new Function("main", instructions)]);
    }
    
    protected static Program GetExpected(List<Function> functions) 
        => new(functions);
    
    protected static Mov Mov(IOperand src, IOperand dest) => 
        new(src, dest);
    
    protected static Imm<int> Imm(int value) =>
        new(value);
    
    protected static Call Call(string identifier) => 
        new(identifier);

    protected static Imm<int> Zero { get; } = Imm(0);
    
    protected static Imm<int> One { get; } = Imm(1);
    
    protected static Stack Stack(int value) =>
        new(value);
    
    protected static AllocateStack AllocateStack(int value) =>
        new(value);
    
    protected static Mov MovDiToStack(int value) =>
        new(Di.Register, Stack(value));
    
    protected static Mov MovSiToStack(int value) =>
        new(Si.Register, Stack(value));
    
    protected static Mov MovDxToStack(int value) =>
        new(Dx.Register, Stack(value));
    
    protected static Mov MovCxToStack(int value) =>
        new(Cx.Register, Stack(value));
    
    protected static Mov MovR8ToStack(int value) =>
        new(R8.Register, Stack(value));
    
    protected static Mov MovR9ToStack(int value) =>
        new(R9.Register, Stack(value));
    
    protected static Mov MovR10ToStack(int value) =>
        new(R10.Register, Stack(value));
    
    protected static Mov MovAxToStack(int value) =>
        new(Ax.Register, Stack(value));
    
    protected static Mov MovStackToR10(int value) =>
        new(Stack(value), R10.Register);
    
    protected static Mov MovStackToAx(int value) =>
        new(Stack(value), Ax.Register);
    
    protected static Mov MovConstantToStack(int value, int offset) =>
        new(Imm(value), Stack(offset));
    
    protected static Mov MovConstantToDi(int value) =>
        new(Imm(value), Di.Register);
    
    protected static Mov MovConstantToSi(int value) =>
        new(Imm(value), Si.Register);
    
    protected static Mov MovConstantToDx(int value) =>
        new(Imm(value), Dx.Register);
    
    protected static Mov MovConstantToCx(int value) =>
        new(Imm(value), Cx.Register);
    
    protected static Mov MovConstantToR8(int value) =>
        new(Imm(value), R8.Register);
    
    protected static Mov MovConstantToR9(int value) =>
        new(Imm(value), R9.Register);
}