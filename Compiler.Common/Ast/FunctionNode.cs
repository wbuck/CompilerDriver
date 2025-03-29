using System.Diagnostics;
using System.Diagnostics.Contracts;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record FunctionNode(
    ReadOnlyMemory<char> Name, 
    ReadOnlyMemory<char> ReturnType, 
    ArgumentListNode? Arguments, 
    INode Body) : INode
{
    public NodeType NodeType => NodeType.Function;

    /*
     * <function> ::= "int" <identifier> "(" "void" ")" "{" <statement> "}"
     */
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var shifted = tokens;
        if (INode.GetTokenAndConsume<KeywordToken>(ref shifted) is not { } keyword)
            return null;

        if (INode.GetTokenAndConsume<IdentifierToken>(ref shifted) is not { } id)
            return null;
        
        if (!INode.CheckTypeAndConsume(shifted, TokenType.OpenParenthesis, out shifted))
            return null;

        var arguments = (!INode.CheckKeywordAndConsume(shifted, "void", out shifted)
            ? ArgumentListNode.Parse(ref shifted, fileContent)
            : null) as ArgumentListNode;
        
        INode.AssertTypeAndConsume(shifted, TokenType.CloseParenthesis, out shifted, fileContent.Span);
        INode.AssertTypeAndConsume(shifted, TokenType.OpenBrace, out shifted, fileContent.Span);

        if (BlockStatementNode.Parse(ref shifted, fileContent) is not { } body)
            throw new FormatException($"Unexpected token: {INode.ReadTokenValue(shifted, fileContent.Span)}");
                
        INode.AssertTypeAndConsume(shifted, TokenType.CloseBrace, out shifted, fileContent.Span);

        tokens = shifted;        
        var name = fileContent.Slice(id.Index, id.Length);
        var returnType = fileContent.Slice(keyword.Index, keyword.Length);
        
        return new FunctionNode(name, returnType, arguments, body);
    }
}