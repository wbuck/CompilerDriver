using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record ReturnNode(INode? Expression) : INode
{
    public NodeType NodeType => NodeType.Return;
    
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!INode.CheckKeywordAndConsume(tokens, "return", out tokens))
            return null;
        
        var expression = ExpressionHelper.Parse(ref tokens, fileContent);
        
        INode.AssertTypeAndConsume(tokens, TokenType.Semicolon, fileContent.Span, out tokens);
        return new ReturnNode(expression);
    }
}