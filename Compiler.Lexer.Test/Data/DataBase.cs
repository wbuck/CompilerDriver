

using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class DataBase : TheoryData<string, List<ExpectedToken>>
{
    protected static ExpectedToken Token(TokenType type, string? value = null) 
        => new(type, value ?? type.ToStringFast(true));
    
    protected static ExpectedToken Token(TokenType type, Keyword keyword) 
        => new(type, keyword.ToStringFast(true));
    
    protected static List<ExpectedToken> GetExpected(IEnumerable<ExpectedToken> tokens)
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