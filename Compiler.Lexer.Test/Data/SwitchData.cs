using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class SwitchData : DataBase
{
    public SwitchData()
    {
        Add
        (
            """
            int main(void) {
                switch(10) {
                    case 1:
                    case 2: { break; }
                    case 10: break;
                    default: break;
                }
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, Keyword.Switch),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "10"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.OpenBrace),
                Token(TokenType.Keyword, Keyword.Case),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Colon),
                Token(TokenType.Keyword, Keyword.Case),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Colon),
                Token(TokenType.OpenBrace),
                Token(TokenType.Keyword, Keyword.Break),
                Token(TokenType.Semicolon),
                Token(TokenType.CloseBrace),
                Token(TokenType.Keyword, Keyword.Case),
                Token(TokenType.NumericConstant, "10"),
                Token(TokenType.Colon),
                Token(TokenType.Keyword, Keyword.Break),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, Keyword.Default),
                Token(TokenType.Colon),
                Token(TokenType.Keyword, Keyword.Break),
                Token(TokenType.Semicolon),
                Token(TokenType.CloseBrace)
            ])
        );
    }
}