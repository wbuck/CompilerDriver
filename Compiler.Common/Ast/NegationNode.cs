using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record NegationNode : INode
{
    public NodeType NodeType => NodeType.Negation;

    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        => INode.CheckTypeAndConsume(tokens, TokenType.Negation, out tokens)
            ? new NegationNode()
            : null;
}