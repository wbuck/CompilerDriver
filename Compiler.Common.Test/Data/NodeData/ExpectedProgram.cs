using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedProgram(List<ExpectedParseResultBase> Nodes)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.Program, node.NodeType);
        var program = Assert.IsType<ProgramNode>(node);
        
        Assert.Equal(Nodes.Count, program.Nodes.Count);        
        foreach (var (actual, expected) in program.Nodes.Zip(Nodes))
            expected.Verify(actual);
    }
}