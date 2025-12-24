using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public record ExpectedToken
(
    TokenType Type,
    string Value
);