namespace Compiler.Common.Tokens;

public record CloseParenthesisToken(int Index) 
    : Token(TokenType.CloseParenthesis, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
        => FindCharacter(value, ')', tokens, static (index, tokens) => tokens.Add(new CloseParenthesisToken(index)));
}