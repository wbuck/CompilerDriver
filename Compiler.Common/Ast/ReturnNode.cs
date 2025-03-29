using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record ReturnNode(INode? Expression) : INode
{
    public NodeType NodeType => NodeType.Return;
    
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (!INode.CheckKeywordAndConsume(tokens, "return", out tokens))
            return null;

        var expression = tokens switch
        {
            var _ when IntegerConstantNode.Parse(ref tokens, fileContent) is { } c => c,
            var _ when FloatConstantNode.Parse(ref tokens, fileContent) is { } c => c,            
            _ => null
        };
        
        INode.AssertTypeAndConsume(tokens, TokenType.Semicolon, out tokens, fileContent.Span);
        return new ReturnNode(expression);
    }
}