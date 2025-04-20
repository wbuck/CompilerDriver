using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data.TokenData;

public record ExpectedToken
(
    TokenType Type,
    string Value
);