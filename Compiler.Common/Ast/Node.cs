using System.Diagnostics.Contracts;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public abstract record Node
{
    public abstract NodeType NodeType { get; }
    
    [Pure]
    protected static bool CheckKeyword(in Span<IToken> tokens, ReadOnlySpan<char> keyword) => 
        !tokens.IsEmpty && 
        tokens[0] is KeywordToken token && 
        token.Keyword.AsSpan().SequenceEqual(keyword);

    protected static bool CheckTypeAndConsume(Span<IToken> tokens, in TokenType tokenType, out Span<IToken> shifted)
    {
        shifted = [];
        return CheckType(tokens, tokenType) && Shift(tokens, out shifted);
    }

    [Pure]
    protected static bool CheckType(in Span<IToken> tokens, in TokenType tokenType, int index = 0) 
        => index > -1 && tokens.Length > index && tokens[index].Type == tokenType;

    protected static bool Shift(Span<IToken> tokens, out Span<IToken> shifted, int amount = 1)
    {
        if (tokens.Length < amount)
        {
            shifted = [];
            return false;
        }
        
        shifted = tokens[amount..];
        return true;
    }
}