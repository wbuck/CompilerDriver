using System.Reflection.Metadata.Ecma335;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record BlockStatementNode(INode[] Body) : INode
{
    public NodeType NodeType => NodeType.BlockStatement;

    /*
     * <statement> ::= "return" <expression> ";"
     */
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (tokens.IsEmpty)
            return null;

        if (INode.CheckKeyword(tokens, "return") &&
            ReturnNode.Parse(ref tokens, fileContent) is { } returnNode)
        {
            return new BlockStatementNode([returnNode]);
        }
        
        return null;
    }
}