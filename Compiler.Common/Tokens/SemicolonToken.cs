namespace Compiler.Common.Tokens;

public record SemiColonToken(int Index) 
    : Token(TokenType.Semicolon, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, ';', tokens, static (index, tokens) => tokens.Add(new SemiColonToken(index)));
}