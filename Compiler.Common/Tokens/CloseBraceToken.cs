namespace Compiler.Common.Tokens;

public record CloseBraceToken(int Index) 
    : Token(TokenType.CloseBrace, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, '}', tokens, static (index, tokens) => tokens.Add(new CloseBraceToken(index)));
}