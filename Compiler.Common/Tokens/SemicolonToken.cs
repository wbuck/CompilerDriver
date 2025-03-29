namespace Compiler.Common.Tokens;

public record SemicolonToken(int Index) : IToken
{
    public TokenType Type => TokenType.Semicolon;
    public int Length => 1;
    
    public static IToken? Parse(ReadOnlySpan<char> value, int offset)
    {
        if (value.IsEmpty || value[0] != ';')
            return null;
        
        return new SemicolonToken(offset);
    }        
}