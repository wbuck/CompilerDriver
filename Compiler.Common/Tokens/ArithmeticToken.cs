namespace Compiler.Common.Tokens;

public record ArithmeticToken(TokenType Type, int Index) : IToken
{
    private static readonly Dictionary<char, TokenType> Symbols = new()
    {
        {'+', TokenType.Plus},
        {'*', TokenType.Multiply},
        {'/', TokenType.Divide},
        {'%', TokenType.Modulo},
        {'!', TokenType.Not},
        {'=', TokenType.Equal},
        {'>', TokenType.GreaterThan},
        {'<', TokenType.LessThan}
    };
    
    public int Length => 1;
    
    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
    {
        if (value.IsEmpty)
            return null;
        
        var token = Symbols.TryGetValue(value[0], out var type) 
            ? new ArithmeticToken(type, offset) 
            : null;
        
        if (token is null)
            return null;
        
        value = value[token.Length..];
        return token;
    }
}