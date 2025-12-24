using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class IfStatementsAndConditionalExpressionsData : DataBase
{
    public IfStatementsAndConditionalExpressionsData()
    {
        Add
        (
            """
            int main(void) {
                int a = 0;
                a = 1 ? 2 : 3;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, Keyword.Int),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.QuestionMark),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Colon),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, Keyword.Return),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 15;
                if (a == 10)
                    return 42;
                else    
                    return 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, Keyword.Int),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "15"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, Keyword.If),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "10"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Keyword, Keyword.Return),
                Token(TokenType.NumericConstant, "42"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, Keyword.Else),
                Token(TokenType.Keyword, Keyword.Return),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
            ])
        );
    }
}