namespace Compiler.Common.Tokens;

public record OpenBraceToken(int Index) 
    : Token(TokenType.OpenBrace, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, '{', tokens, static (index, tokens) => tokens.Add(new OpenBraceToken(index)));
}