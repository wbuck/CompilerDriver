using System.Diagnostics;
using System.Diagnostics.Contracts;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record FunctionNode(string Name, INode[] Arguments, INode Body) : INode
{
    public NodeType NodeType => NodeType.Function;

    /*
     * <function> ::= "int" <identifier> "(" "void" ")" "{" <statement> "}"
     */
    public static FunctionNode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!INode.CheckTypeAndConsume(tokens, TokenType.Keyword, out var shifted))
            return null;
        
        if (!INode.CheckTypeAndConsume(shifted, TokenType.Identifier, out shifted))
            return null;
        
        if (!INode.CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;
        
        var arguments = ArgumentNode.Parse(ref shifted, fileContent);
        
        if (!INode.CheckTypeAndConsume(shifted, TokenType.CloseParenthesis, out shifted))
            return null;
        
        if (!INode.CheckTypeAndConsume(shifted, TokenType.OpenBrace, out shifted))
            return null;

        // TODO: parse statement
        
        return null;
    }
}