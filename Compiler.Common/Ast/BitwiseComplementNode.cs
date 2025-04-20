using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public record BitwiseComplementNode : INode
{
    public NodeType NodeType => NodeType.BitwiseComplement;
    
    public static INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        => INode.CheckTypeAndConsume(tokens, TokenType.BitwiseComplement, out tokens)
            ? new BitwiseComplementNode()
            : null;
}