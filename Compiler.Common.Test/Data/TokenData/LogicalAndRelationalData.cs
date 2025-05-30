using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data.TokenData;

public class LogicalAndRelationalData : DataBase
{
    public LogicalAndRelationalData()
    {
        Add
        (
            """
            int main(void) {
                return (10 && 0) + (0 && 4) + (0 && 0);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "10"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 && (1 / 0);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.ForwardSlash),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 && -1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 >= 0 > 1 <= 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.GreaterThanOrEqual),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.GreaterThan),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LessThanOrEqual),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~2 * -2 == 1 + 5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Complement),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Asterisk),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Plus),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 3 == 1 != 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.NotEqual),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 == 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 >= 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThanOrEqual),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 >= 1) + (1 >= -4);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThanOrEqual),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThanOrEqual),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 > 2) + (1 > 1);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThan),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThan),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 15 > 10;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "15"),
                Token(TokenType.GreaterThan),
                Token(TokenType.NumericConstant, "10"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 <= -1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LessThanOrEqual),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 <= 2) + (0 <= 0);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LessThanOrEqual),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LessThanOrEqual),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 < 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.LessThan),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 < 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LessThan),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 || 0 && (1 / 0);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.ForwardSlash),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 0 != 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.NotEqual),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return -1 != -2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.NotEqual),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return !-3;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Not),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(3 - 44);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Not),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "44"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return !(4-4);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Not),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return !0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Not),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return !5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Not),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return ~(0 && 1) - -(4 || 3);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Complement),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Minus),
                Token(TokenType.Minus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 || (1 / 0);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LogicalOr),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.ForwardSlash),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (4 || 0) + (0 || 3) + (5 || 5);
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (1 || 0) && 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 >= 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.GreaterThanOrEqual),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 2 == 2 || 0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return (0 == 0 && 3 == 2 + 1 > 1) + 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Plus),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.GreaterThan),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Plus),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 1 || 0 && 2;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.LogicalOr),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 & 7 == 5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.BitwiseAnd),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 | 7 != 5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.BitwiseOr),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.NotEqual),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 20 >> 4 <= 3 << 1;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "20"),
                Token(TokenType.RightShift),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.LessThanOrEqual),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.LeftShift),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                return 5 ^ 7 < 5;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.BitwiseXor),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.LessThan),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon)
            ])
        );
    }
}