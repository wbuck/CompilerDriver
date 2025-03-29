namespace Compiler.Common.Tokens;

public record KeywordToken(string Value) : Token(TokenType.Keyword);