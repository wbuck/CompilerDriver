using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedNegation : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.Negation, node.NodeType);
        Assert.IsType<NegationNode>(node);
    }
}