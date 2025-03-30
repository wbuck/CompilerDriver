namespace Compiler.Common.Tokens;

public record OpenParenthesisToken(int Index) 
    : Token(TokenType.OpenParenthesis, Index, 1)
{
    public static void Parse(ReadOnlySpan<char> value, in List <Token> tokens)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (value[index] == '(')
                tokens.Add(new OpenParenthesisToken(index));
        }
    }
}