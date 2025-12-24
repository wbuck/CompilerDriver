using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class CompoundOperatorData : DataBase
{
    public CompoundOperatorData()
    {
        Add
        (
            """
            int main(void) {
                int a = 15;
                int b = a ^ 5;
                return 1 | b;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "15"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Assignment),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.BitwiseXor),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.BitwiseOr),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 250;
                int b = 200;
                int c = 100;
                int d = 75;
                int e = -25;
                int f = 0;
                int x = 0;
                x = a += b -= c *= d /= e %= f = -7;
                return a == 2250 && b == 2000 && c == -1800 && d == -18 && e == -4 &&
                       f == -7 && x == 2250;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "250"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "200"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "c"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "100"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "d"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "75"),
                Token(TokenType.Semicolon),                
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "e"),
                Token(TokenType.Assignment),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "25"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "f"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "x"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "x"),
                Token(TokenType.Assignment),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.AdditionAssignment),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.SubtractionAssignment),
                Token(TokenType.Identifier, "c"),
                Token(TokenType.MultiplicationAssignment),
                Token(TokenType.Identifier, "d"),
                Token(TokenType.DivisionAssignment),
                Token(TokenType.Identifier, "e"),
                Token(TokenType.RemainderAssignment),
                Token(TokenType.Identifier, "f"),
                Token(TokenType.Assignment),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2250"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2000"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "c"),
                Token(TokenType.Equal),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "1800"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "d"),
                Token(TokenType.Equal),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "18"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "e"),
                Token(TokenType.Equal),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "4"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "f"),
                Token(TokenType.Equal),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "7"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.Identifier, "x"),
                Token(TokenType.Equal),
                Token(TokenType.NumericConstant, "2250"),
                Token(TokenType.Semicolon),
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a |= 30;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.BitwiseOrAssignment),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a &= 30;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.BitwiseAndAssignment),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a ^= 30;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.BitwiseXorAssignment),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a <<= 30;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.LeftShiftAssignment),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a >>= 30;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.RightShiftAssignment),
                Token(TokenType.NumericConstant, "30"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a++;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Increment),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                ++a;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Increment),
                Token(TokenType.Identifier, "a"),                
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                a--;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Decrement),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 1;
                --a;
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Decrement),
                Token(TokenType.Identifier, "a"),                
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
    }
}