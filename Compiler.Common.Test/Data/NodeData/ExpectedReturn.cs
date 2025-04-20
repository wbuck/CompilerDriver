using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedReturn(ExpectedParseResultBase? Expression)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.Return, node.NodeType);
        var ret = Assert.IsType<ReturnNode>(node);

        if (Expression is not null)
        {
            Assert.NotNull(ret.Expression);
            Expression.Verify(ret.Expression);
            return;
        }
        Assert.Null(ret.Expression);
    }
}