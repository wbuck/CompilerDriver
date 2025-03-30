using System.Numerics;

namespace Compiler.Common.Tokens;

public record NumericConstant<T>(T Value, int Length)
    : Token(TokenType.Constant, Length) where T : INumber<T>
{
    
}