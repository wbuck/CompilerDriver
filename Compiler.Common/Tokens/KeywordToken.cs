namespace Compiler.Common.Tokens;

public sealed record KeywordToken(int Index, int Length, Keyword Keyword) : IToken
{
    public TokenType Type => TokenType.Keyword;

    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
        => IdentifierToken.Parse(ref value, offset);
}