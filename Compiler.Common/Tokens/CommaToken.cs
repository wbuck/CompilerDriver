namespace Compiler.Common.Tokens;

public record CommaToken(int Index) 
    : Token(TokenType.Comma, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, ',', tokens, static (index, tokens) => tokens.Add(new CommaToken(index)));
}