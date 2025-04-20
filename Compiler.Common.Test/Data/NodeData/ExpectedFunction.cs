using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public sealed record ExpectedFunction(string ReturnType, string Name, ExpectedParseResultBase Body)
    : ExpectedParseResultBase
{
    public override void Verify(INode node)
    {
        Assert.Equal(NodeType.Function, node.NodeType);
        var function = Assert.IsType<FunctionNode>(node);
        
        Assert.Equal(ReturnType.AsMemory(), function.ReturnType);
        Assert.Equal(Name.AsMemory(), function.Name);
        Body.Verify(function.Body);
    }
}