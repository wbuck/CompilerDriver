using System.ComponentModel.DataAnnotations;
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
    Function,
    [Display(Name = "e")]
    Equal,
    [Display(Name = "ne")]
    NotEqual,
    [Display(Name = "l")]
    LessThan,
    [Display(Name = "le")]
    LessThanOrEqual,
    [Display(Name = "g")]
    GreaterThan,
    [Display(Name = "ge")]
    GreaterThanOrEqual,
    Cmp,
    Jmp,
    JmpConditional,
    SetConditional,
    Label
}

public interface IAssembly
{
    AssemblyTag Tag { get; }
}

public interface IOperand : IAssembly
{
    static Imm<int> Zero { get; } = new(0);
}

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
public sealed record Cx : IReg
{
    public static Cx Register { get; } = new();
    private Cx() { }
    public AssemblyTag Tag => AssemblyTag.Cx;
}

public interface IConstant : IOperand;
public sealed record Imm<T>(T Constant) : IConstant where T: INumber<T>
{
    public AssemblyTag Tag => AssemblyTag.Imm;
}

public sealed record Pseudo(string Identifier): IOperand
{
    public AssemblyTag Tag => AssemblyTag.Pseudo;
}

public sealed record Stack(int Offset) : IOperand
{
    public AssemblyTag Tag => AssemblyTag.Stack;
}

public interface IUnaryOperator : IAssembly;
public sealed record Neg : IUnaryOperator
{
    public static Neg Operator { get; } = new();
    private Neg() { }
    public AssemblyTag Tag => AssemblyTag.Neg;
}
public sealed record Not : IUnaryOperator
{
    public static Not Operator { get; } = new();
    private Not() { }
    public AssemblyTag Tag => AssemblyTag.Not;
}

public interface IInstruction : IAssembly;
public sealed record Mov(IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Mov;   
}

public sealed record Unary(IUnaryOperator Operator, IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Unary;  
}

public sealed record AllocateStack(int Offset) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.AllocateStack; 
}

public sealed record Ret : IInstruction
{
    public static Ret Instruction { get; } = new();
    private Ret() { }
    public AssemblyTag Tag => AssemblyTag.Ret;
}
public sealed record Cdq : IInstruction
{
    public static Cdq Instruction { get; } = new();
    private Cdq() { }
    public AssemblyTag Tag => AssemblyTag.Cdq;   
}
public sealed record Binary(IBinaryOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Binary; 
}
public sealed record Bitwise(IBitwiseOperator Operator, IOperand Source, IOperand Destination) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Bitwise; 
}
public sealed record Div(IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Div;
}
public sealed record Cmp(IOperand Lhs, IOperand Rhs) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Cmp;
}
public sealed record Jmp(string Target) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Jmp;
}
public sealed record JmpConditional(IConditionCode Code, string Target) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.JmpConditional;
}
public sealed record SetConditional(IConditionCode Code, IOperand Operand) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.SetConditional;
}
public sealed record Label(string Identifier) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.Label;
}

public interface IBitwiseOperator : IAssembly;
public sealed record BitwiseAnd : IBitwiseOperator
{
    public static BitwiseAnd Operator { get; } = new();
    private BitwiseAnd() { }
    public AssemblyTag Tag => AssemblyTag.And;
}
public sealed record BitwiseOr : IBitwiseOperator
{
    public static BitwiseOr Operator { get; } = new();
    private BitwiseOr() { }
    public AssemblyTag Tag => AssemblyTag.Or;
}
public sealed record BitwiseXor : IBitwiseOperator
{
    public static BitwiseXor Operator { get; } = new();
    private BitwiseXor() { }
    public AssemblyTag Tag => AssemblyTag.Xor;
}
public sealed record BitwiseLeftShift : IBitwiseOperator
{
    public static BitwiseLeftShift Operator { get; } = new();
    private BitwiseLeftShift() { }
    public AssemblyTag Tag => AssemblyTag.LeftShift;
}
public sealed record BitwiseRightShift : IBitwiseOperator
{
    public static BitwiseRightShift Operator { get; } = new();
    private BitwiseRightShift() { }
    public AssemblyTag Tag => AssemblyTag.RightShift;
}

public interface IBinaryOperator : IAssembly;
public sealed record Add : IBinaryOperator
{
    public static Add Operator { get; } = new();
    private Add() { }
    public AssemblyTag Tag => AssemblyTag.Add;  
}
public sealed record Sub : IBinaryOperator
{
    public static Sub Operator { get; } = new();
    private Sub() { }
    public AssemblyTag Tag => AssemblyTag.Sub;
}
public sealed record Mult : IBinaryOperator
{
    public static Mult Operator { get; } = new();
    private Mult() { }
    public AssemblyTag Tag => AssemblyTag.Mult;
}

public interface IConditionCode : IAssembly;
public sealed record Equal : IConditionCode
{
    public static Equal Code { get; } = new();
    private Equal() { }
    public AssemblyTag Tag => AssemblyTag.Equal;
}
public sealed record NotEqual : IConditionCode
{
    public static NotEqual Code { get; } = new();
    private NotEqual() { }
    public AssemblyTag Tag => AssemblyTag.NotEqual;
}
public sealed record GreaterThan : IConditionCode
{
    public static GreaterThan Code { get; } = new();
    private GreaterThan() { }
    public AssemblyTag Tag => AssemblyTag.GreaterThan;
}
public sealed record GreaterThanOrEqual : IConditionCode
{
    public static GreaterThanOrEqual Code { get; } = new();
    private GreaterThanOrEqual() { }
    public AssemblyTag Tag => AssemblyTag.GreaterThanOrEqual;
}
public sealed record LessThan : IConditionCode
{
    public static LessThan Code { get; } = new();
    private LessThan() { }
    public AssemblyTag Tag => AssemblyTag.LessThan;
}
public sealed record LessThanOrEqual : IConditionCode
{
    public static LessThanOrEqual Code { get; } = new();
    private LessThanOrEqual() { }
    public AssemblyTag Tag => AssemblyTag.LessThanOrEqual;
}

public sealed record Function(string Name, List<IInstruction> Instructions): IAssembly
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
            TackyJump jump => VisitJump(jump),
            TackyJumpIfZero jumpIfZero => VisitJumpIfZero(jumpIfZero),
            TackyJumpIfNotZero jumpIfNotZero => VisitJumpIfNotZero(jumpIfNotZero),
            TackyCopy copy => VisitCopy(copy),
            TackyLabel label => VisitLabel(label),
            _ => throw new FormatException($"Unknown instruction type {i.Tag.ToStringFast()}")
        }).ToList();
    
    private static List<IInstruction> VisitLabel(TackyLabel label)
        => [new Label(label.Identifier)];
    
    private static List<IInstruction> VisitCopy(TackyCopy copy) 
        => [new Mov(VisitValue(copy.Source), VisitValue(copy.Destination))];
    
    private static List<IInstruction> VisitJumpIfNotZero(TackyJumpIfNotZero jump)
        => [
            new Cmp(IOperand.Zero, VisitValue(jump.Condition)),
            new JmpConditional(NotEqual.Code, jump.Target)
        ];

    private static List<IInstruction> VisitJumpIfZero(TackyJumpIfZero jump)
        => [
            new Cmp(IOperand.Zero, VisitValue(jump.Condition)),
            new JmpConditional(Equal.Code, jump.Target)
        ];
    
    private static List<IInstruction> VisitJump(TackyJump jump) 
        => [new Jmp(jump.Target)];

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
        return binary switch
        {
            { Operator: TackyDivision or TackyRemainder } => GetDivOrRemainderInstructions(binary),
            _ when IsRelational(binary.Operator) => GetRelationalInstructions(binary), 
            _ => GetArithmeticInstructions(binary)
        };

        static IEnumerable<IInstruction> GetRelationalInstructions(TackyBinary binary)
        {
            var dest = VisitValue(binary.Destination);
            return [
                new Cmp(VisitValue(binary.Lhs), VisitValue(binary.Rhs)),
                new Mov(IOperand.Zero, dest),
                new SetConditional(GetConditionCode(binary.Operator), dest),
            ];
        }
        
        static IEnumerable<IInstruction> GetDivOrRemainderInstructions(TackyBinary binary) =>
        [
            new Mov(VisitValue(binary.Lhs), Ax.Register),
            Cdq.Instruction,
            new Div(VisitValue(binary.Rhs)),
            new Mov(GetDivResultRegister(binary.Operator), VisitValue(binary.Destination))
        ];

        static IEnumerable<IInstruction> GetArithmeticInstructions(TackyBinary binary)
        {
            var dest = VisitValue(binary.Destination);
            return [
                new Mov(VisitValue(binary.Lhs), dest),
                new Binary(GetBinaryOperator(binary.Operator), VisitValue(binary.Rhs), dest)
            ];
        }
        
        static bool IsRelational(ITackyBinaryOperator binary) =>
            binary is TackyLessThan 
                or TackyLessThanOrEqual 
                or TackyGreaterThan 
                or TackyGreaterThanOrEqual 
                or TackyEqual 
                or TackyNotEqual;
        
        static IReg GetDivResultRegister(ITackyBinaryOperator binary) =>
            binary is TackyDivision 
                ? Ax.Register 
                : Dx.Register;
    }

    private static IConditionCode GetConditionCode(ITackyBinaryOperator op)
        => op switch
        {
            TackyEqual => Equal.Code,
            TackyNotEqual => NotEqual.Code,
            TackyLessThan => LessThan.Code,
            TackyLessThanOrEqual => LessThanOrEqual.Code,
            TackyGreaterThan => GreaterThan.Code,
            TackyGreaterThanOrEqual => GreaterThanOrEqual.Code,
            _ => throw new FormatException($"Unknown condition code type {op.Tag.ToStringFast()}")
        };

    private static IEnumerable<IInstruction> VisitUnary(TackyUnary unary)
    {
        return unary switch
        {
            { Operator: TackyNot } => GetNotInstructions(unary),
            _ => GetOtherInstructions(unary)
        };

        static IEnumerable<IInstruction> GetNotInstructions(TackyUnary unary)
        {
            var dest = VisitValue(unary.Destination);
            return [
                new Cmp(IOperand.Zero, VisitValue(unary.Source)),
                new Mov(IOperand.Zero, dest),
                new SetConditional(Equal.Code, dest),
            ];
        }

        static IEnumerable<IInstruction> GetOtherInstructions(TackyUnary unary)
        {
            var dest = VisitValue(unary.Destination);
            return [
                new Mov(VisitValue(unary.Source), dest),
                new Unary(GetUnaryOperator(unary.Operator), dest)
            ];  
        }            
    }

    private static IOperand VisitValue(ITackyValue value)
        => value switch
        {
            TackyConstant<int> integer => new Imm<int>(integer.Value),
            TackyConstant<double> floating => new Imm<double>(floating.Value),
            TackyVariable variable => new Pseudo(variable.Identifier),
            _ => throw new FormatException($"Unknown operand type {value.Tag.ToStringFast()}")
        };
    
    private static List<IInstruction> VisitReturn(TackyReturn @return)
        => [new Mov(VisitValue(@return.Value), Ax.Register), Ret.Instruction];

    private static IBitwiseOperator GetBitwiseOperator(ITackyBitwiseOperator bitwise)
        => bitwise switch
        {
            TackyBitwiseAnd => BitwiseAnd.Operator,
            TackyBitwiseOr => BitwiseOr.Operator,
            TackyBitwiseXor => BitwiseXor.Operator,
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