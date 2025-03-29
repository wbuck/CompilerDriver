using System.Numerics;

namespace Compiler.Common.Tokens;

public record ConstantToken<T>(T Value) 
    : Token(TokenType.Constant) where T : INumber<T>;