namespace Compiler.Lexer.Tokens;

public sealed record PlusToken(int Index) : IToken
{
    public TokenType Type => TokenType.Plus;
    public int Length => 1;
    
    public static IToken? Parse(ref ReadOnlySpan<char> value, int offset)
    {
        if (value.IsEmpty || value[0] != '+')
            return null;
        
        value = value[1..];
        return new PlusToken(offset);
    }
}