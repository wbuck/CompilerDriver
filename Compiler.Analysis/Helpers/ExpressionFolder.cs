using System.Diagnostics;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Helpers;

public static class ExpressionFolder
{
    public static int FoldExpression(IExpressionNode expression)
        => expression switch
        {
            BinaryNode node => Binary(node),
            UnaryNode node => Unary(node),
            BitwiseNode node => Bitwise(node),
            ConstantNode<int> node => node.Value,
            _ => throw new UnreachableException()
        };

    private static int Bitwise(BitwiseNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);

        return node.Operator switch
        {
            BitwiseAndNode => lhs & rhs,
            BitwiseOrNode => lhs | rhs,
            BitwiseXorNode => lhs ^ rhs,
            BitwiseLeftShiftNode => lhs << rhs,
            BitwiseRightShiftNode => lhs >> rhs,
            _ => throw new UnreachableException()
        };
    }
    
    private static int Unary(UnaryNode node)
    {
        var constant = FoldExpression(node.Expression);
        
        return node.Operator switch
        {
            NegateNode => -constant,
            ComplementNode => ~constant,
            NotNode => constant == 0 ? 1 : 0,
            _ => throw new UnreachableException()
        };
    }

    private static int Binary(BinaryNode node)
    {
        var lhs = FoldExpression(node.Lhs);
        var rhs = FoldExpression(node.Rhs);

        return node.Operator switch
        {
            AdditionNode => lhs + rhs,
            SubtractionNode => lhs - rhs,
            MultiplicationNode => lhs * rhs,
            DivisionNode => lhs / rhs,
            RemainderNode => lhs % rhs,
            _ => throw new UnreachableException()
        };
    }
}