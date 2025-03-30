namespace Compiler.Common.Tokens;

public record ArithmeticToken(TokenType Type, int Index) : IToken
{
    private static readonly Dictionary<char, TokenType> Symbols = new()
    {
        {'+', TokenType.Plus},
        {'-', TokenType.Minus},
        {'*', TokenType.Multiply},
        {'/', TokenType.Divide},
        {'%', TokenType.Modulo},
        {'!', TokenType.Not},
        {'=', TokenType.Equal},
        {'>', TokenType.GreaterThan},
        {'<', TokenType.LessThan}
    };
    
    public int Length => 1;
    
    public static IToken? Parse(ReadOnlySpan<char> value, int offset)
    {
        if (value.IsEmpty)
            return null;
        
        return Symbols.TryGetValue(value[0], out var type) 
            ? new ArithmeticToken(type, offset) 
            : null;
    }
}