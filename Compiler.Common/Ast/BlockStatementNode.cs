using System.Reflection.Metadata.Ecma335;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record BlockStatementNode(INode[] Body) : INode
{
    public NodeType NodeType => NodeType.BlockStatement;

    public static BlockStatementNode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        return null;
    }
}