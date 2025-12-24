using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class LocalVariableData : DataBase
{
    public LocalVariableData()
    {
        Add
        (
            """
            int main(void) {
                int first_variable = 1;
                int second_variable = 2;
                return first_variable + second_variable;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "first_variable"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "second_variable"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "first_variable"),
                Token(TokenType.Plus),
                Token(TokenType.Identifier, "second_variable"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int var0;
                var0 = 2;
                return var0;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "var0"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "var0"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "2"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "var0"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = -2593;
                a = a % 3;
                int b = -a;
                return b;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.Minus),
                Token(TokenType.NumericConstant, "2593"),
                Token(TokenType.Semicolon),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Percent),
                Token(TokenType.NumericConstant, "3"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Assignment),
                Token(TokenType.Minus),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "b"),
                Token(TokenType.Semicolon)
            ])
        );
        Add
        (
            """
            int main(void) {
                int a = 0;
                0 || (a = 1);
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalOr),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "1"),
                Token(TokenType.CloseParenthesis),
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
                int a = 0;
                0 && (a = 5);
                return a;
            }
            """,
            GetExpected([
                Token(TokenType.Keyword, "int"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.Semicolon),
                Token(TokenType.NumericConstant, "0"),
                Token(TokenType.LogicalAnd),
                Token(TokenType.OpenParenthesis),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Assignment),
                Token(TokenType.NumericConstant, "5"),
                Token(TokenType.CloseParenthesis),
                Token(TokenType.Semicolon),
                Token(TokenType.Keyword, "return"),
                Token(TokenType.Identifier, "a"),
                Token(TokenType.Semicolon)
            ])
        );
    }
}