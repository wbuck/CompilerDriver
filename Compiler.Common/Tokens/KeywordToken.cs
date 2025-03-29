namespace Compiler.Common.Tokens;

public record KeywordToken(int Index, int Length, string Keyword) : IToken
{
    public TokenType Type => TokenType.Keyword;
}