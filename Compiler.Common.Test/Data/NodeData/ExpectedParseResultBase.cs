using Compiler.Common.Ast;

namespace Compiler.Common.Test.Data.NodeData;

public abstract record ExpectedParseResultBase
{
    public abstract void Verify(INode node);
}