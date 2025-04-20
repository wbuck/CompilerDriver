using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedIntegerConstant(int Value)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.IntegerConstant, node.NodeType);
        var integer = Assert.IsType<IntegerConstantNode>(node);
        Assert.Equal(Value, integer.Value);
    }
}