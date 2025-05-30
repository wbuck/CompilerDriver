using System.Diagnostics;
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
    BitwiseAnd,
    BitwiseOr,
    BitwiseXor,
    LeftShift,
    RightShift,
    Not,
    LogicalAnd,
    LogicalOr,
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    Copy,
    Jump,
    JumpIfZero,
    JumpIfNotZero,
    Label
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

public sealed record TackyCopy(ITackyValue Source, ITackyValue Destination) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Copy;
}
public sealed record TackyJump(string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Jump;
}
public sealed record TackyJumpIfZero(ITackyValue Condition, string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.JumpIfZero;
}
public sealed record TackyJumpIfNotZero(ITackyValue Condition, string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.JumpIfNotZero;
}
public sealed record TackyLabel(string Identifier) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Label;
}

public interface ITackyValue : ITackyTag
{
    public static TackyConstant<int> True { get; } = new(1);
    public static TackyConstant<int> False { get; } = new(0);
}
public sealed record TackyConstant<T>(T Value) : ITackyValue where T : INumber<T>
{
    public TackyTag Tag => TackyTag.Constant;    
}

public sealed record TackyVariable(string Identifier) : ITackyValue
{
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
public sealed record TackyNot: ITackyUnaryOperator
{
    public static TackyNot Operator { get; } = new();
    private TackyNot() { }
    public TackyTag Tag => TackyTag.Not;
}

public interface ITackyBitwiseOperator : ITackyTag;
public sealed record TackyBitwiseAnd : ITackyBitwiseOperator
{
    public static TackyBitwiseAnd Operator { get; } = new();
    private TackyBitwiseAnd() { }
    public TackyTag Tag => TackyTag.BitwiseAnd;
}
public sealed record TackyBitwiseOr : ITackyBitwiseOperator
{
    public static TackyBitwiseOr Operator { get; } = new();
    private TackyBitwiseOr() { }
    public TackyTag Tag => TackyTag.BitwiseOr;
}
public sealed record TackyBitwiseXor : ITackyBitwiseOperator
{
    public static TackyBitwiseXor Operator { get; } = new();
    private TackyBitwiseXor() { }
    public TackyTag Tag => TackyTag.BitwiseXor;
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
public sealed record TackyEqual : ITackyBinaryOperator
{
    public static TackyEqual Operator { get; } = new();
    private TackyEqual() { }
    public TackyTag Tag => TackyTag.Equal;
}
public sealed record TackyNotEqual : ITackyBinaryOperator
{
    public static TackyNotEqual Operator { get; } = new();
    private TackyNotEqual() { }
    public TackyTag Tag => TackyTag.NotEqual;
}
public sealed record TackyLessThan : ITackyBinaryOperator
{
    public static TackyLessThan Operator { get; } = new();
    private TackyLessThan() { }
    public TackyTag Tag => TackyTag.LessThan;
}
public sealed record TackyLessThanOrEqual : ITackyBinaryOperator
{
    public static TackyLessThanOrEqual Operator { get; } = new();
    private TackyLessThanOrEqual() { }
    public TackyTag Tag => TackyTag.LessThanOrEqual;
}
public sealed record TackyGreaterThan : ITackyBinaryOperator
{
    public static TackyGreaterThan Operator { get; } = new();
    private TackyGreaterThan() { }
    public TackyTag Tag => TackyTag.GreaterThan;
}
public sealed record TackyGreaterThanOrEqual : ITackyBinaryOperator
{
    public static TackyGreaterThanOrEqual Operator { get; } = new();
    private TackyGreaterThanOrEqual() { }
    public TackyTag Tag => TackyTag.GreaterThanOrEqual;
}

public sealed record TackyFunction(string Name, List<ITackyInstruction> Instructions) : ITackyTag
{
    public TackyTag Tag => TackyTag.Function;
}

public sealed record TackyProgram(TackyFunction Function) : ITackyTag
{
    public TackyTag Tag => TackyTag.Program;    
}

public class TackyVisitor
{
    private readonly LabelGenerator _labelGenerator = new();
    
    public TackyProgram Visit(ProgramNode program)
        => new(VisitFunction(program.Function));

    private TackyFunction VisitFunction(FunctionNode function)
    {
        List<ITackyInstruction> instructions = [];
        VariableFactory factory = new();
        foreach (var item in function.Body)
        {
            switch (item)
            {
                case DeclarationNode node:
                    VisitDeclaration(node, instructions, factory);
                    break;
                case IStatementNode statement:
                    VisitStatement(statement, instructions, factory);
                    break;
                default:
                    throw new UnreachableException($"Unknown block item type: {item.Tag.ToStringFast()}");
            }
        }
        
        instructions.Add(new TackyReturn(new TackyConstant<int>(0)));
        return new TackyFunction(function.Name, instructions);
    }

    private List<ITackyInstruction> VisitDeclaration(
        DeclarationNode declaration, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        if (declaration is not { Initializer: { } rhs }) 
            return instructions;
        
        var lhs = new VariableNode(declaration.Identifier);
        VisitAssignment(lhs, rhs, instructions, factory);
        return instructions;
    }

    private List<ITackyInstruction> VisitStatement(
        IStatementNode statement, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        switch (statement)
        {
            case ReturnNode ret:
                return VisitReturn(ret, instructions, factory);
            case ExpressionNode expr:
                // We don't care about the result of the expression statement
                // in this case.
                _ = VisitExpression(expr.Expression, instructions, factory);
                return instructions;
            case NullNode:
                return instructions;
            default:
                throw new UnreachableException($"Unknown statement type: {statement.Tag.ToStringFast()}");
        }
    } 

    private List<ITackyInstruction> VisitReturn(ReturnNode @return, List<ITackyInstruction> instructions, VariableFactory factory)
    {        
        instructions.Add(new TackyReturn(VisitExpression(@return.Expression, instructions, factory)));
        return instructions;
    }
    
    private ITackyValue VisitExpression(
        IExpressionNode expression, in List<ITackyInstruction> instructions, VariableFactory factory)
        => expression switch
        {
            IConstantNode constant => VisitConstant(constant),
            UnaryNode unary => VisitUnary(unary, instructions, factory),            
            BitwiseNode bitwise => VisitBitwise(bitwise, instructions, factory),
            BinaryNode binary => VisitBinary(binary, instructions, factory),
            VariableNode variable => factory.GetNextVariable(variable.Identifier),
            AssignmentNode assignment => VisitAssignment(assignment, instructions, factory),
            _ => throw new FormatException($"Unknown expression type: {expression.Tag.ToStringFast()}")
        };

    private TackyVariable VisitAssignment(
        AssignmentNode assignment, List<ITackyInstruction> instructions, VariableFactory factory)
            => VisitAssignment(assignment.Lhs, assignment.Rhs, instructions, factory);

    private TackyVariable VisitAssignment(
        IExpressionNode lhs, IExpressionNode rhs, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var left = VisitExpression(lhs, instructions, factory);
        var right = VisitExpression(rhs, instructions, factory);        
        
        // This should never happen as the semantic analysis stage should
        // have handled this already.
        Debug.Assert(left is TackyVariable, $"Invalid assignment type: {left.Tag.ToStringFast()}");
        
        var variable = (TackyVariable)left;
        instructions.Add(new TackyCopy(right, variable));
        return variable;
    }

    private TackyVariable VisitBitwise(
        BitwiseNode bitwise, in List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var lhs = VisitExpression(bitwise.Lhs, instructions, factory);
        var rhs = VisitExpression(bitwise.Rhs, instructions, factory);
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyBitwise(GetBitwiseOperator(bitwise), lhs, rhs, dest));
        return dest;
      
    }
    
    private TackyVariable VisitBinaryLogicalOr(
        BinaryNode binary, in List<ITackyInstruction> instructions, VariableFactory factory)
    {                        
        var trueLabel = _labelGenerator.GetNextLabel(TackyConstants.OR_WHEN_NOT_ZERO_LABEL);        
        
        instructions.Add(new TackyJumpIfNotZero(VisitExpression(binary.Lhs, instructions, factory), trueLabel));
        instructions.Add(new TackyJumpIfNotZero(VisitExpression(binary.Rhs, instructions, factory), trueLabel));
        
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.OR_END_LABEL);  
        var result = factory.GetNextVariable();
        
        instructions.Add(new TackyCopy(ITackyValue.False, result));
        instructions.Add(new TackyJump(endLabel));
                      
        instructions.Add(new TackyLabel(trueLabel));                       
        instructions.Add(new TackyCopy(ITackyValue.True, result));  
        instructions.Add(new TackyLabel(endLabel));       
        
        return result;
    }

    private TackyVariable VisitBinaryLogicalAnd(
        BinaryNode binary, in List<ITackyInstruction> instructions, VariableFactory factory)
    {                        
        var falseLabel = _labelGenerator.GetNextLabel(TackyConstants.AND_WHEN_ZERO_LABEL);        
        
        instructions.Add(new TackyJumpIfZero(VisitExpression(binary.Lhs, instructions, factory), falseLabel));
        instructions.Add(new TackyJumpIfZero(VisitExpression(binary.Rhs, instructions, factory), falseLabel));
        
        var endLabel = _labelGenerator.GetNextLabel(TackyConstants.AND_END_LABEL);  
        var result = factory.GetNextVariable();
        
        instructions.Add(new TackyCopy(ITackyValue.True, result));
        instructions.Add(new TackyJump(endLabel));
                      
        instructions.Add(new TackyLabel(falseLabel));                       
        instructions.Add(new TackyCopy(ITackyValue.False, result));  
        instructions.Add(new TackyLabel(endLabel));       
        
        return result;
    }
    
    private TackyVariable VisitBinary(BinaryNode binary, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        return binary switch
        {
            { Operator: LogicalAndNode } => VisitBinaryLogicalAnd(binary, instructions, factory),
            { Operator: LogicalOrNode } => VisitBinaryLogicalOr(binary, instructions, factory),
            _ => VisitBinaryInternal()
        };
        
        TackyVariable VisitBinaryInternal()
        {
            var lhs = VisitExpression(binary.Lhs, instructions, factory);
            var rhs = VisitExpression(binary.Rhs, instructions, factory);    
            var dest = factory.GetNextVariable();
        
            instructions.Add(new TackyBinary(GetBinaryOperator(binary), lhs, rhs, dest));
            return dest;
        }
    }
    
    private TackyVariable VisitUnary(UnaryNode unary, List<ITackyInstruction> instructions, VariableFactory factory)
    {
        var source = VisitExpression(unary.Expression, instructions, factory);   
        var dest = factory.GetNextVariable();
        
        instructions.Add(new TackyUnary(GetUnaryOperator(unary), source, dest));
        return dest;
    }
    
    private static ITackyUnaryOperator GetUnaryOperator(UnaryNode unary)
        => unary.Operator switch
        {
            ComplementNode => TackyComplement.Operator,
            NegateNode => TackyNegate.Operator,
            NotNode => TackyNot.Operator,
            _ => throw new FormatException($"Unknown unary operator: {unary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBinaryOperator GetBinaryOperator(BinaryNode binary)
        => binary.Operator switch
        {
            AdditionNode => TackyAddition.Operator,
            SubtractionNode => TackySubtraction.Operator,
            MultiplicationNode => TackyMultiplication.Operator,
            DivisionNode => TackyDivision.Operator,
            RemainderNode => TackyRemainder.Operator,
            EqualNode => TackyEqual.Operator,
            NotEqualNode => TackyNotEqual.Operator,
            LessThanNode => TackyLessThan.Operator,
            LessThanOrEqualNode => TackyLessThanOrEqual.Operator,
            GreaterThanNode => TackyGreaterThan.Operator,
            GreaterThanOrEqualNode => TackyGreaterThanOrEqual.Operator,
            _ => throw new FormatException($"Unknown binary operator: {binary.Operator.Tag.ToStringFast()}")
        };
    
    private static ITackyBitwiseOperator GetBitwiseOperator(BitwiseNode bitwise)
        => bitwise.Operator switch
        {
            BitwiseAndNode => TackyBitwiseAnd.Operator,
            BitwiseOrNode => TackyBitwiseOr.Operator,
            BitwiseXorNode => TackyBitwiseXor.Operator,
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