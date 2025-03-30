namespace Compiler.Common.Tokens;

public record CloseBraceToken(int Index) : IToken
{
    public TokenType Type => TokenType.CloseBrace;
    public int Length => 1;
    
    public static void Parse(ReadOnlySpan<char> value, in List<IToken> tokens)
        => IToken.FindCharacter(value, '}', tokens, 
            static (index, tokens) => tokens.Add(new CloseBraceToken(index)));
}