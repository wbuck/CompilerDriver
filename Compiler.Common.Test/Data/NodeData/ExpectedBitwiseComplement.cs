using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedBitwiseComplement : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.BitwiseComplement, node.NodeType);
        Assert.IsType<BitwiseComplementNode>(node);
    }
}