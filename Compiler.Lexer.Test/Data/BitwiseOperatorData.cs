using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class BitwiseOperatorData : DataBase
{
    public BitwiseOperatorData()
    {
        Add
        (
            """
            int main(void) {
                return 3 & 5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.BitwiseAnd),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)                
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 | 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.BitwiseOr),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)                
            ])
        );
        Add
        (
            """
            int main(void) {
                return 80 >> 2 | 1 ^ 5 & 7 << 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "80"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.BitwiseOr),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.BitwiseXor),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.BitwiseAnd),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)                
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 >> 2 << 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "33"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 33 << 4 >> 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "33"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 40 << 4 + 12 >> 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "40"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.Plus),
                Token(TokenType.NumericConstant, "12"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 35 << 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "35"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return -5 >> 30;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1000 >> 4;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1000"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 << (2 * 2)) + (100 >> (1 + 2));
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.LeftShift),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Asterisk),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "100"),
                Token(TokenType.RightShift),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Plus),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 7 ^ 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.BitwiseXor),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
    }
}