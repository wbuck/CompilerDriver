using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedFloatConstant(double Value)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.FloatConstant, node.NodeType);
        var floating = Assert.IsType<FloatConstantNode>(node);
        Assert.Equal(Value, floating.Value);
    }
}