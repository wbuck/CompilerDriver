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
            ConstantNode<int> node => node,
            _ => null
        };

    private static ConstantNode<int>? Bitwise(BitwiseNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);
        
        if (lhs is null || rhs is null) return null;

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
        var constant = FoldExpression(node.Expression);
        if (constant is null) return null;
        
        return node.Operator switch
        {
            NegateNode => Const(-constant.Value),
            ComplementNode => Const(~constant.Value),
            NotNode => Const(constant.Value == 0 ? 1 : 0),
            _ => throw new UnreachableException()
        };
    }

    private static ConstantNode<int>? Binary(BinaryNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);
        
        if (lhs is null || rhs is null) return null;

        return node.Operator switch
        {
            AdditionNode => Const(lhs.Value + rhs.Value),
            SubtractionNode => Const(lhs.Value - rhs.Value),
            MultiplicationNode => Const(lhs.Value * rhs.Value),
            DivisionNode => Const(lhs.Value / rhs.Value),
            RemainderNode => Const(lhs.Value % rhs.Value),
            _ => throw new UnreachableException()
        };
    }
    
    private static ConstantNode<int> Const(int value) => new(value);
}