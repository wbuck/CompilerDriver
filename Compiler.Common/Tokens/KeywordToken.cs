namespace Compiler.Common.Tokens;

public record KeywordToken(string Value, int Index, int Length)
    : Token(TokenType.Keyword, Index, Length);