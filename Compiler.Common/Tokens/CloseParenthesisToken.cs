namespace Compiler.Common.Tokens;

public record CloseParenthesisToken(int Index) 
    : Token(TokenType.CloseParenthesis, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (value[index] == ')')
                tokens.Add(new CloseParenthesisToken(index));
        }
    }
}