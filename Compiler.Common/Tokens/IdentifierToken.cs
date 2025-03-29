namespace Compiler.Common.Tokens;

public record IdentifierToken(string Value) : Token(TokenType.Identifier);