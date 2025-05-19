using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data.TokenData;

public class DataBase : TheoryData<string, List<ExpectedToken>>
{
    protected static ExpectedToken Token(TokenType type, string? value = null) 
        => new(type, value ?? type.ToStringFast(true));
    
    protected static List<ExpectedToken> Expected(IEnumerable<ExpectedToken> tokens)
        => [
            Token(TokenType.Keyword, "int"),
            Token(TokenType.Identifier, "main"),
            Token(TokenType.OpenParenthesis),
            Token(TokenType.Keyword, "void"),
            Token(TokenType.CloseParenthesis),
            Token(TokenType.OpenBrace),
            .. tokens,
            Token(TokenType.CloseBrace)
        ];
}