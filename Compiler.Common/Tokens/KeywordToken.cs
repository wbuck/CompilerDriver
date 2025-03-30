namespace Compiler.Common.Tokens;

public record KeywordToken(string Value, int Length) 
    : Token(TokenType.Keyword, Length);