using System.Diagnostics;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Helpers;

public static class ExpressionFolder
{
    public static ConstantNode<int>? FoldExpression(IExpressionNode expression)
        => expression switch
        {
            BinaryNode node => Binary(node),
            UnaryNode node => Unary(node),
            BitwiseNode node => Bitwise(node), 
            ConditionalNode node => Conditional(node),
            ConstantNode<int> node => node,
            _ => null
        };

    private static ConstantNode<int>? Conditional(ConditionalNode node)
    {
        var condition = FoldExpression(node.Condition);
        var trueBranch = FoldExpression(node.True);
        var falseBranch = FoldExpression(node.False);
        
        if (condition is null || trueBranch is null || falseBranch is null) 
            return null;
        
        return Const(condition.Value != 0 ? trueBranch.Value : falseBranch.Value);
    }

    private static ConstantNode<int>? Bitwise(BitwiseNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);
        
        if (lhs is null || rhs is null) 
            return null;

        return node.Operator switch
        {
            BitwiseAndNode => Const(lhs.Value & rhs.Value),
            BitwiseOrNode => Const(lhs.Value | rhs.Value),
            BitwiseXorNode => Const(lhs.Value ^ rhs.Value),
            BitwiseLeftShiftNode => Const(lhs.Value << rhs.Value),
            BitwiseRightShiftNode => Const(lhs.Value >> rhs.Value),
            _ => throw new UnreachableException()
        };                
    }    
    
    private static ConstantNode<int>? Unary(UnaryNode node)
    {
        if (FoldExpression(node.Expression) is not { } constant) 
            return null;
        
        return node.Operator switch
        {
            NegateNode => Const(-constant.Value),
            ComplementNode => Const(~constant.Value),
            NotNode => Const(constant.Value == 0 ? 1 : 0),
            _ => null
        };
    }

    private static ConstantNode<int>? Binary(BinaryNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);
        
        if (lhs is null || rhs is null) 
            return null;

        return node.Operator switch
        {
            AdditionNode => Const(lhs.Value + rhs.Value),
            SubtractionNode => Const(lhs.Value - rhs.Value),
            MultiplicationNode => Const(lhs.Value * rhs.Value),
            NotEqualNode => Const(lhs.Value != rhs.Value ? 1 : 0),
            DivisionNode => Const(lhs.Value / rhs.Value),
            EqualNode => Const(lhs.Value == rhs.Value ? 1 : 0),
            GreaterThanNode => Const(lhs.Value > rhs.Value ? 1 : 0),
            GreaterThanOrEqualNode => Const(lhs.Value >= rhs.Value ? 1 : 0),
            LessThanNode => Const(lhs.Value < rhs.Value ? 1 : 0),
            LessThanOrEqualNode => Const(lhs.Value <= rhs.Value ? 1 : 0),
            LogicalAndNode => Const(lhs.Value != 0 && rhs.Value != 0 ? 1 : 0),
            LogicalOrNode => Const(lhs.Value != 0 || rhs.Value != 0 ? 1 : 0),
            RemainderNode => Const(lhs.Value % rhs.Value),            
            _ => throw new UnreachableException()
        };
    }
    
    private static ConstantNode<int> Const(int value) => new(value);
}