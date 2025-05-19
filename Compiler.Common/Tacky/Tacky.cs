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
    Program,
    Bitwise,
    And,
    Or,
    Xor,
    LeftShift,
    RightShift,
}

public interface ITackyTag
{
    TackyTag Tag { get; }
}

public interface ITackyInstruction : ITackyTag;
public sealed record TackyReturn(ITackyValue Value) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Return;
}

public sealed record TackyUnary(ITackyUnaryOperator Operator, ITackyValue Source, ITackyValue Destination) 
    : ITackyInstruction
{
    public TackyTag Tag  => TackyTag.Unary;
}
public sealed record TackyBinary(ITackyBinaryOperator Operator, ITackyValue Lhs, ITackyValue Rhs, ITackyValue Destination)
    : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Binary;
}
public sealed record TackyBitwise(ITackyBitwiseOperator Operator, ITackyValue Lhs, ITackyValue Rhs, ITackyValue Destination)
    : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Bitwise;
}

public interface ITackyValue : ITackyTag;
public sealed record TackyConstant<T>(T Value) : ITackyValue where T : INumber<T>
{
    public TackyTag Tag => TackyTag.Constant;
}

public sealed record TackyVariable : ITackyValue
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
public sealed record TackyComplement: ITackyUnaryOperator
{
    public static TackyComplement Operator { get; } = new();
    private TackyComplement() { }
    public TackyTag Tag => TackyTag.Complement;
}
public sealed record TackyNegate: ITackyUnaryOperator
{
    public static TackyNegate Operator { get; } = new();
    private TackyNegate() { }
    public TackyTag Tag => TackyTag.Negate;
}

public interface ITackyBitwiseOperator : ITackyTag;
public sealed record TackyAnd : ITackyBitwiseOperator
{
    public static TackyAnd Operator { get; } = new();
    private TackyAnd() { }
    public TackyTag Tag => TackyTag.And;
}
public sealed record TackyOr : ITackyBitwiseOperator
{
    public static TackyOr Operator { get; } = new();
    private TackyOr() { }
    public TackyTag Tag => TackyTag.Or;
}
public sealed record TackyXor : ITackyBitwiseOperator
{
    public static TackyXor Operator { get; } = new();
    private TackyXor() { }
    public TackyTag Tag => TackyTag.Xor;
}
public sealed record TackyLeftShift : ITackyBitwiseOperator
{
    public static TackyLeftShift Operator { get; } = new();
    private TackyLeftShift() { }
    public TackyTag Tag => TackyTag.LeftShift;
}
public sealed record TackyRightShift : ITackyBitwiseOperator
{
    public static TackyRightShift Operator { get; } = new();
    private TackyRightShift() { }
    public TackyTag Tag => TackyTag.RightShift;
}

public interface ITackyBinaryOperator  : ITackyTag;
public sealed record TackyAddition : ITackyBinaryOperator
{
    public static TackyAddition Operator { get; } = new();
    private TackyAddition() { }
    public TackyTag Tag  => TackyTag.Addition;
}

public sealed record TackySubtraction : ITackyBinaryOperator
{
    public static TackySubtraction Operator { get; } = new();
    private TackySubtraction() { }
    public TackyTag Tag   => TackyTag.Subtraction;
}
public sealed record TackyMultiplication : ITackyBinaryOperator
{
    public static TackyMultiplication Operator { get; } = new();
    private TackyMultiplication() { }
    public TackyTag Tag => TackyTag.Multiplication;
}
public sealed record TackyDivision : ITackyBinaryOperator
{
    public static TackyDivision Operator { get; } = new();
    private TackyDivision() { }
    public TackyTag Tag => TackyTag.Division;
}
public sealed record TackyRemainder : ITackyBinaryOperator
{
    public static TackyRemainder Operator { get; } = new();
    private TackyRemainder() { }
    public TackyTag Tag => TackyTag.Remainder;
}

public sealed record TackyFunction(string Name, List<ITackyInstruction> Instructions) : ITackyTag
{
    public TackyTag Tag => TackyTag.Function;
}

public sealed record TackyProgram(TackyFunction Function) : ITackyTag
{
    public TackyTag Tag => TackyTag.Program;
    
    public static TackyProgram Visit(ProgramNode program)
        => new(VisitFunction(program.Function));

    private static TackyFunction VisitFunction(FunctionNode function) =>
        new(function.Name, VisitStatement(function.Body, new VariableFactory()));

    private static List<ITackyInstruction> VisitStatement(IStatementNode statement, VariableFactory factory)
        => statement switch
        {
            ReturnNode @return => VisitReturn(@return, factory),
            _ => throw new FormatException($"Unknown statement type {statement.Tag.ToStringFast()}")
        };    

    private static List<ITackyInstruction> VisitReturn(ReturnNode @return, VariableFactory factory)
    {        
        List<ITackyInstruction> instructions = [];
        instructions.Add(new TackyReturn(VisitExpression(@return.Expression, instructions, factory)));
        return instructions;
    }
    
    private static ITackyValue VisitExpression(IExpressionNode expression, in List<ITackyInstruction> instructions, VariableFactory factory)
        => expression switch
        {
            IConstantNode constant => VisitConstant(constant),
            UnaryNode unary => VisitUnary(unary, instructions, factory),
            BinaryNode binary => VisitBinary(binary, instructions, factory),
            BitwiseNode bitwise => VisitBitwise(bitwise, instructions, factory),
            _ => throw new FormatException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private static TackyVariable VisitBitwise(BitwiseNode bitwise, in List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var lhs = VisitExpression(bitwise.Lhs, instructions, factory);
        var rhs = VisitExpression(bitwise.Rhs, instructions, factory);
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyBitwise(GetOperator(bitwise), lhs, rhs, dest));
        return dest;
      
    }
    
    private static TackyVariable VisitBinary(BinaryNode binary, in List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var lhs = VisitExpression(binary.Lhs, instructions, factory);
        var rhs = VisitExpression(binary.Rhs, instructions, factory);    
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyBinary(GetOperator(binary), lhs, rhs, dest));
        return dest;
    }
    
    private static TackyVariable VisitUnary(UnaryNode unary, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var source = VisitExpression(unary.Expression, instructions, factory);   
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyUnary(GetOperator(unary), source, dest));
        return dest;
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
            _ => throw new FormatException($"Unknown binary operator: {binary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBitwiseOperator GetOperator(BitwiseNode bitwise)
        => bitwise.Operator switch
        {
            BitwiseAndNode => TackyAnd.Operator,
            BitwiseOrNode => TackyOr.Operator,
            BitwiseXorNode => TackyXor.Operator,
            BitwiseLeftShiftNode => TackyLeftShift.Operator,
            BitwiseRightShiftNode => TackyRightShift.Operator,
            _ => throw new FormatException($"Unknown bitwise operator: {bitwise.Operator.Tag.ToStringFast()}")
        };

    private static ITackyValue VisitConstant(IConstantNode constant)
        => constant switch
        {
            ConstantNode<int> integer => new TackyConstant<int>(integer.Value),
            ConstantNode<double> floating => new TackyConstant<double>(floating.Value),
            _ => throw new FormatException($"Unknown constant node type: {constant.Tag.ToStringFast()}")
        };    
}

