using System.Numerics;
using Compiler.Common.Tacky;
using NetEscapades.EnumGenerators;

namespace Compiler.Common.Generation;

[EnumExtensions]
public enum AssemblyTag
{
    Mov,
    Add,
    Sub,
    Mult,
    Div,
    Neg,
    Not,
    Cdq,
    Ax,
    R10,
    R11,
    Dx,
    Cx,
    Cl,
    Imm,
    Pseudo,
    Stack,
    Ret,
    AllocateStack,
    Binary,
    Unary,
    Bitwise,
    And,
    Or,
    Xor,
    LeftShift,
    RightShift,
    Program,
    Function
}

public interface IAssembly
{
    AssemblyTag Tag { get; }
}

public interface IOperand : IAssembly;

public interface IReg : IOperand;
public sealed record Ax : IReg
{
    public static Ax Register { get; } = new();
    private Ax() { }
    public AssemblyTag Tag => AssemblyTag.Ax;
}
public sealed record R10 : IReg
{
    public static R10 Register { get; } = new();
    private R10() { }
    public AssemblyTag Tag => AssemblyTag.R10;
}
public sealed record R11 : IReg
{
    public static R11 Register { get; } = new();
    private R11() { }
    public AssemblyTag Tag => AssemblyTag.R11;
}
public sealed record Dx : IReg
{
    public static Dx Register { get; } = new();
    private Dx() { }
    public AssemblyTag Tag => AssemblyTag.Dx;
}
public sealed record Cl : IReg
{
    public static Cl Register { get; } = new();
    private Cl() { }
    public AssemblyTag Tag => AssemblyTag.Cl;
}
public sealed record Cx : IReg
{
    public static Cx Register { get; } = new();
    private Cx() { }
    public AssemblyTag Tag => AssemblyTag.Cx;
}

public record Imm<T>(T Constant) : IOperand where T: INumber<T>
{
    public AssemblyTag Tag => AssemblyTag.Imm;
}

public record Pseudo(string Identifier, int StackOffset): IOperand
{
    public AssemblyTag Tag => AssemblyTag.Pseudo;
}

public record Stack(int Offset) : IOperand
{
    public AssemblyTag Tag => AssemblyTag.Stack;
}

public interface IUnaryOperator : IAssembly;
public record Neg : IUnaryOperator
{
    public static Neg Operator { get; } = new();
    private Neg() { }
    public AssemblyTag Tag => AssemblyTag.Neg;
}
public record Not : IUnaryOperator
{
    public static Not Operator { get; } = new();
    private Not() { }
    public AssemblyTag Tag => AssemblyTag.Not;
}

public interface IInstruction : IAssembly;
public record Mov(IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Mov;   
}

public record Unary(IUnaryOperator Operator, IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Unary;  
}

public record AllocateStack(int Offset) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.AllocateStack; 
}

public record Ret : IInstruction
{
    public static Ret Instruction { get; } = new();
    private Ret() { }
    public AssemblyTag Tag => AssemblyTag.Ret;
}
public record Cdq : IInstruction
{
    public static Cdq Instruction { get; } = new();
    private Cdq() { }
    public AssemblyTag Tag => AssemblyTag.Cdq;   
}
public record Binary(IBinaryOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Binary; 
}
public record Bitwise(IBitwiseOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Bitwise; 
}

public record Div(IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Div;
}

public interface IBitwiseOperator : IAssembly;

public record BitwiseAnd : IBitwiseOperator
{
    public static BitwiseAnd Operator { get; } = new();
    private BitwiseAnd() { }
    public AssemblyTag Tag => AssemblyTag.And;
}
public record BitwiseOr : IBitwiseOperator
{
    public static BitwiseOr Operator { get; } = new();
    private BitwiseOr() { }
    public AssemblyTag Tag => AssemblyTag.Or;
}
public record BitwiseXor : IBitwiseOperator
{
    public static BitwiseXor Operator { get; } = new();
    private BitwiseXor() { }
    public AssemblyTag Tag => AssemblyTag.Xor;
}
public record BitwiseLeftShift : IBitwiseOperator
{
    public static BitwiseLeftShift Operator { get; } = new();
    private BitwiseLeftShift() { }
    public AssemblyTag Tag => AssemblyTag.LeftShift;
}
public record BitwiseRightShift : IBitwiseOperator
{
    public static BitwiseRightShift Operator { get; } = new();
    private BitwiseRightShift() { }
    public AssemblyTag Tag => AssemblyTag.RightShift;
}

public interface IBinaryOperator : IAssembly;
public record Add : IBinaryOperator
{
    public static Add Operator { get; } = new();
    private Add() { }
    public AssemblyTag Tag => AssemblyTag.Add;  
}
public record Sub : IBinaryOperator
{
    public static Sub Operator { get; } = new();
    private Sub() { }
    public AssemblyTag Tag => AssemblyTag.Sub;
}
public record Mult : IBinaryOperator
{
    public static Mult Operator { get; } = new();
    private Mult() { }
    public AssemblyTag Tag => AssemblyTag.Mult;
}

public record Function(string Name, List<IInstruction> Instructions): IAssembly
{
    public AssemblyTag Tag => AssemblyTag.Function;   
}

public record Program(Function Function): IAssembly
{
    public static Program Visit(TackyProgram tacky)
    {
        PseudoReplacer replacer = new();
        
        var program = replacer.Replace(new Program(VisitFunction(tacky.Function)));
        program.Function.Instructions.Insert(0, new AllocateStack(replacer.StackOffset));
        
        return InvalidInstructionReplacer.Replace(program);
    }

    private static Function VisitFunction(TackyFunction function)
        => new(function.Name, VisitInstructions(function.Instructions));
    
    private static List<IInstruction> VisitInstructions(IEnumerable<ITackyInstruction> instructions)
        => instructions.SelectMany(i => i switch
        {
            TackyUnary unary => VisitUnary(unary),
            TackyReturn ret => VisitReturn(ret),
            TackyBinary binary => VisitBinary(binary),
            TackyBitwise bitwise => VisitBitwise(bitwise),
            _ => throw new FormatException($"Unknown instruction type {i.Tag.ToStringFast()}")
        }).ToList();

    private static IEnumerable<IInstruction> VisitBitwise(TackyBitwise bitwise)
    {
        var dest = VisitValue(bitwise.Destination);
        return [
            new Mov(VisitValue(bitwise.Lhs), dest),
            new Bitwise(GetBitwiseOperator(bitwise.Operator), VisitValue(bitwise.Rhs), dest)
        ];
    }
    private static IEnumerable<IInstruction> VisitBinary(TackyBinary binary)
    {
        if (binary.Operator is TackyDivision or TackyRemainder)
        {
            return [
                new Mov(VisitValue(binary.Lhs), Ax.Register),
                Cdq.Instruction,
                new Div(VisitValue(binary.Rhs)),
                new Mov(GetDivResultRegister(binary.Operator), VisitValue(binary.Destination))
            ];

            static IReg GetDivResultRegister(ITackyBinaryOperator binary) =>
                binary is TackyDivision 
                    ? Ax.Register 
                    : Dx.Register;
        }

        var dest = VisitValue(binary.Destination);
        return [
            new Mov(VisitValue(binary.Lhs), dest),
            new Binary(GetBinaryOperator(binary.Operator), VisitValue(binary.Rhs), dest)
        ];
    }
    
    private static IEnumerable<IInstruction> VisitUnary(TackyUnary unary)
        => [
            new Mov(VisitValue(unary.Source), VisitValue(unary.Destination)),
            new Unary(GetUnaryOperator(unary.Operator), VisitValue(unary.Destination))
        ];
    
    private static IOperand VisitValue(ITackyValue value)
        => value switch
        {
            TackyConstant<int> integer => new Imm<int>(integer.Value),
            TackyConstant<double> floating => new Imm<double>(floating.Value),
            TackyVariable variable => new Pseudo(variable.Identifier, variable.StackOffset),
            _ => throw new FormatException($"Unknown operand type {value.Tag.ToStringFast()}")
        };
    
    private static List<IInstruction> VisitReturn(TackyReturn @return)
        => [new Mov(VisitValue(@return.Value), Ax.Register), Ret.Instruction];

    private static IBitwiseOperator GetBitwiseOperator(ITackyBitwiseOperator bitwise)
        => bitwise switch
        {
            TackyAnd => BitwiseAnd.Operator,
            TackyOr => BitwiseOr.Operator,
            TackyXor => BitwiseXor.Operator,
            TackyLeftShift => BitwiseLeftShift.Operator,
            TackyRightShift => BitwiseRightShift.Operator,
            _ => throw new FormatException($"Unknown bitwise operator type {bitwise.Tag.ToStringFast()}")
        };
    
    private static IBinaryOperator GetBinaryOperator(ITackyBinaryOperator binary)
        => binary switch
        {
            TackyAddition => Add.Operator,
            TackySubtraction => Sub.Operator,
            TackyMultiplication => Mult.Operator,            
            _ => throw new FormatException($"Unknown binary operator type {binary.Tag.ToStringFast()}")
        };
    
    private static IUnaryOperator GetUnaryOperator(ITackyUnaryOperator unary)
        => unary switch
        {
            TackyNegate => Neg.Operator,
            TackyComplement => Not.Operator,
            _ => throw new FormatException($"Unknown unary operator type {unary.Tag.ToStringFast()}")
        };


    public AssemblyTag Tag => AssemblyTag.Program;
}