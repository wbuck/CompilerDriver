using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedUnaryOperator(ExpectedParseResultBase Unary, ExpectedParseResultBase Expression)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.UnaryOperator, node.NodeType);
        var unary = Assert.IsType<UnaryOperatorNode>(node);
        Unary.Verify(unary.Unary);
        Expression.Verify(unary.Expression);
    }
}