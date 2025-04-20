using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record UnaryOperatorNode(INode Unary, INode Expression) : INode
{
    public NodeType NodeType => NodeType.UnaryOperator;
    
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (NegationNode.Parse(ref tokens, fileContent) is { } minus)
            return new UnaryOperatorNode(minus, GetExpression(ref tokens, fileContent));

        if (BitwiseComplementNode.Parse(ref tokens, fileContent) is { } bitwise)
            return new UnaryOperatorNode(bitwise, GetExpression(ref tokens, fileContent));

        return null;
    }

    private static INode GetExpression(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        if (ExpressionHelper.Parse(ref tokens, fileContent) is not { } node)
            throw new FormatException($"Expected expression but found '{INode.ReadTokenValue(tokens, fileContent.Span)}'");
        
        return node;
    }
}