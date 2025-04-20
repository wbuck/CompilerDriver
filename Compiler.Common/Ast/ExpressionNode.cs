using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public static class ExpressionHelper
{
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        INode? node = null;
        if (IntegerConstantNode.Parse(ref tokens, fileContent) is { } integer)
            node = integer;

        if (FloatConstantNode.Parse(ref tokens, fileContent) is { } floating)
            node = floating;

        if (UnaryOperatorNode.Parse(ref tokens, fileContent) is { } unary)
            node = unary;

        if (INode.CheckTypeAndConsume(tokens, TokenType.OpenParenthesis, out tokens))
        {
            node = Parse(ref tokens, fileContent);
            INode.AssertTypeAndConsume(tokens, TokenType.CloseParenthesis, fileContent.Span, out tokens);
        }                

        return node;
    }
}