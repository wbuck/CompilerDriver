using Compiler.Common.Tokens;

namespace Compiler.Common.Extensions;

public static class TokenTypeExtensions
{
    public static bool IsUnaryOperator(this TokenType type)
        => type is TokenType.Not 
                or TokenType.Complement 
                or TokenType.Increment 
                or TokenType.Decrement;
}