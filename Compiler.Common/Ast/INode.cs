using System.Diagnostics.Contracts;
using Compiler.Common.Tokens;

namespace Compiler.Common.Ast;

public interface INode
{
    NodeType NodeType { get; }
    
    static virtual INode? Parse(ref Span<IToken> tokens, ReadOnlyMemory<char> fileContent)
        => null;
    
    protected static TToken? GetTokenAndConsume<TToken>(ref Span<IToken> tokens) 
        where TToken : class, IToken
    {
        if (GetToken<TToken>(tokens) is not { } token)
            return null;

        tokens = tokens[1..];
        return token;
    }
    
    protected static TToken? GetToken<TToken>(in Span<IToken> tokens) 
        where TToken : class, IToken
    {
        if (tokens.IsEmpty)
            return null;
        
        return tokens[0] as TToken;
    }
    
    [Pure]
    protected static bool CheckKeyword(in Span<IToken> tokens, in ReadOnlySpan<char> keyword) => 
        !tokens.IsEmpty && 
        tokens[0] is KeywordToken token && 
        token.Keyword.AsSpan().SequenceEqual(keyword);

    protected static void AssertTypeAndConsume(
        Span<IToken> tokens, TokenType tokenType, out Span<IToken> shifted, ReadOnlySpan<char> fileContent)
    {
        if (CheckTypeAndConsume(tokens, tokenType, out shifted))
            return;
        
        if (tokens.IsEmpty)
            throw new FormatException($"Missing '{tokenType.ToStringFast(true)}'");
        
        var value = ReadTokenValue(tokens, fileContent);
        throw new FormatException($"Unexpected token: {value}");
    }

    protected static bool CheckTypeAndConsume(Span<IToken> tokens, TokenType tokenType, out Span<IToken> shifted)
    {
        shifted = tokens;
        return CheckType(tokens, tokenType) && Shift(tokens, out shifted);
    }
    
    protected static bool CheckKeywordAndConsume(Span<IToken> tokens, ReadOnlySpan<char> keyword, out Span<IToken> shifted)
    {
        shifted = tokens;
        return CheckKeyword(tokens, keyword) && Shift(tokens, out shifted);
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

    [Pure]
    protected static ReadOnlySpan<char> ReadTokenValue(in Span<IToken> tokens, in ReadOnlySpan<char> fileContent)
        => tokens.IsEmpty ? default : fileContent.Slice(tokens[0].Index, tokens[0].Length);
}