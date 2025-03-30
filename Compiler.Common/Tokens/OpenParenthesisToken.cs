namespace Compiler.Common.Tokens;

public record OpenParenthesisToken(int Index) 
    : Token(TokenType.OpenParenthesis, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, '(', tokens, static (index, tokens) => tokens.Add(new OpenParenthesisToken(index)));
}