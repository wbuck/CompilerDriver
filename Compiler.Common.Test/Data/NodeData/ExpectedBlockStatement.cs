using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedBlockStatement(List<ExpectedParseResultBase> Body)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.BlockStatement, node.NodeType);
        var statement = Assert.IsType<BlockStatementNode>(node);
        
        Assert.Equal(Body.Count, statement.Body.Length);
        foreach (var (actual, expected) in statement.Body.Zip(Body))
            expected.Verify(actual);
    }
}