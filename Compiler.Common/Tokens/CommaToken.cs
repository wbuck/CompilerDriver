namespace Compiler.Common.Tokens;

public record CommaToken(int Index) : IToken
{
    public TokenType Type => TokenType.Comma;
    public int Length => 1;
    
    public static void Parse(ReadOnlySpan<char> value, in List<IToken> tokens)
        => IToken.FindCharacter(value, ',', tokens, static (index, tokens) => tokens.Add(new CommaToken(index)));
}