using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.InteropServices;
using Compiler.Common.Ast;
using NetEscapades.EnumGenerators;

namespace Compiler.Common.Tacky;

[EnumExtensions]
public enum TackyTag
{
    Return,
    Unary,
    Binary,
    Constant,
    Variable,
    Complement,
    Negate,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Remainder,
    Function,
    Program
}

public interface ITackyTag
{
    TackyTag Tag { get; }
}

public interface ITackyInstruction : ITackyTag;
public record TackyReturn(ITackyValue Value) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Return;
}

public record TackyUnary(ITackyUnaryOperator Operator, ITackyValue Source, ITackyValue Destination) 
    : ITackyInstruction
{
    public TackyTag Tag  => TackyTag.Unary;
}

public record TackyBinary(ITackyBinaryOperator Operator, ITackyValue Lhs, ITackyValue Rhs, ITackyValue Destination)
    : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Binary;
}

public interface ITackyValue : ITackyTag;
public record TackyConstant<T>(T Value) : ITackyValue where T : INumber<T>
{
    public TackyTag Tag => TackyTag.Constant;
}

public record TackyVariable : ITackyValue
{
    private readonly int _varCount;
    public TackyVariable(int varCount)
    {
        Identifier = $"tmp.{varCount}";
        StackOffset = varCount * Marshal.SizeOf<int>();
        _varCount = varCount;
    }
    public string Identifier { get; }   
    public int StackOffset { get; }
    public TackyVariable Next() => new(_varCount + 1);
    public TackyTag Tag => TackyTag.Variable;
}

public interface ITackyUnaryOperator : ITackyTag;
public record TackyComplement: ITackyUnaryOperator
{
    public static TackyComplement Operator { get; } = new();
    private TackyComplement() { }
    public TackyTag Tag => TackyTag.Complement;
}
public record TackyNegate: ITackyUnaryOperator
{
    public static TackyNegate Operator { get; } = new();
    private TackyNegate() { }
    public TackyTag Tag => TackyTag.Negate;
}

public interface ITackyBinaryOperator  : ITackyTag;
public record TackyAddition : ITackyBinaryOperator
{
    public static TackyAddition Operator { get; } = new();
    private TackyAddition() { }
    public TackyTag Tag  => TackyTag.Addition;
}

public record TackySubtraction : ITackyBinaryOperator
{
    public static TackySubtraction Operator { get; } = new();
    private TackySubtraction() { }
    public TackyTag Tag   => TackyTag.Subtraction;
}
public record TackyMultiplication : ITackyBinaryOperator
{
    public static TackyMultiplication Operator { get; } = new();
    private TackyMultiplication() { }
    public TackyTag Tag => TackyTag.Multiplication;
}
public record TackyDivision : ITackyBinaryOperator
{
    public static TackyDivision Operator { get; } = new();
    private TackyDivision() { }
    public TackyTag Tag => TackyTag.Division;
}
public record TackyRemainder : ITackyBinaryOperator
{
    public static TackyRemainder Operator { get; } = new();
    private TackyRemainder() { }
    public TackyTag Tag => TackyTag.Remainder;
}

public record TackyFunction(string Name, List<ITackyInstruction> Instructions) : ITackyTag
{
    public TackyTag Tag => TackyTag.Function;
}

public record TackyProgram(TackyFunction Function) : ITackyTag
{
    public TackyTag Tag => TackyTag.Program;
    
    public static TackyProgram Visit(ProgramNode program)
        => new(VisitFunction(program.Function));

    private static TackyFunction VisitFunction(FunctionNode function)
        => new(function.Name, VisitStatement(function.Body));

    private static List<ITackyInstruction> VisitStatement(IStatementNode statement)
        => statement switch
        {
            ReturnNode @return => VisitReturn(@return),
            _ => throw new FormatException($"Unknown statement type {statement.Tag.ToStringFast()}")
        };    

    private static List<ITackyInstruction> VisitReturn(ReturnNode @return)
    {        
        List<ITackyInstruction> instructions = [];
        instructions.Add(new TackyReturn(VisitExpression(@return.Expression, instructions)));
        return instructions;
    }
    
    private static ITackyValue VisitExpression(IExpressionNode expression, in List<ITackyInstruction> instructions)
        => expression switch
        {
            IConstantNode constant => VisitConstant(constant),
            UnaryNode unary => VisitUnary(unary, instructions),
            BinaryNode binary => VisitBinary(binary, instructions),
            _ => throw new FormatException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private static TackyVariable? GetLastUsedVariable(in List<ITackyInstruction> instructions)
        => instructions.LastOrDefault() switch
        {
            TackyBinary binary => binary.Destination,
            TackyUnary unary => unary.Destination,
            _ => null
        } as TackyVariable;
    

    private static TackyVariable VisitBinary(BinaryNode binary, in List<ITackyInstruction> instructions)
    {
        var lhs = VisitExpression(binary.Lhs, instructions);
        var rhs = VisitExpression(binary.Rhs, instructions);
        
        TackyVariable dest;
        if (binary is { Lhs: IConstantNode, Rhs: IConstantNode })
        {
            dest = GetLastUsedVariable(instructions)?.Next() ?? new TackyVariable(1);
            instructions.Add(new TackyBinary(GetOperator(binary), lhs, rhs, dest));
            return dest;
        }

        dest = GetNextDest(lhs, rhs);        
        instructions.Add(new TackyBinary(GetOperator(binary), lhs, rhs, dest));
        return dest;
        
        static TackyVariable GetNextDest(ITackyValue lhs, ITackyValue rhs) => (lhs, rhs) switch
        {
            (TackyVariable v, _) => v.Next(),
            (_, TackyVariable v) => v.Next(),
            _ => throw new FormatException($"Expected variable but found {lhs.Tag.ToStringFast()} and {rhs.Tag.ToStringFast()}")    
        };
    }
    
    private static TackyVariable VisitUnary(UnaryNode unary, List<ITackyInstruction> instructions)
    {
        var source = VisitExpression(unary.Expression, instructions);
        
        TackyVariable dest;
        if (unary.Expression is IConstantNode)
        {
            dest = new TackyVariable(1);
            instructions.Add(new TackyUnary(GetOperator(unary),source, dest));
            return dest;
        }

        dest = GetNextDest(source);
        instructions.Add(new TackyUnary(GetOperator(unary), source, dest));
        return dest;
        
        static TackyVariable GetNextDest(ITackyValue source) => source switch
        {
            TackyVariable v => v.Next(),
            _ => throw new FormatException($"Expected variable but found {source.Tag.ToStringFast()}")    
        };
    }
    
    private static ITackyUnaryOperator GetOperator(UnaryNode unary)
        => unary.Operator switch
        {
            ComplementNode => TackyComplement.Operator,
            NegateNode => TackyNegate.Operator,
            _ => throw new FormatException($"Unknown unary operator: {unary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBinaryOperator GetOperator(BinaryNode binary)
        => binary.Operator switch
        {
            AdditionNode => TackyAddition.Operator,
            SubtractionNode => TackySubtraction.Operator,
            MultiplicationNode => TackyMultiplication.Operator,
            DivisionNode => TackyDivision.Operator,
            RemainderNode => TackyRemainder.Operator,
            _ => throw new FormatException($"Unknown unary operator: {binary.Operator.Tag.ToStringFast()}")
        };

    private static ITackyValue VisitConstant(IConstantNode constant)
        => constant switch
        {
            ConstantNode<int> integer => new TackyConstant<int>(integer.Value),
            ConstantNode<double> floating => new TackyConstant<double>(floating.Value),
            _ => throw new FormatException($"Unknown constant node type: {constant.Tag.ToStringFast()}")
        };    
}

