using Compiler.Lexer.Tokens;

namespace Compiler.Lexer.Test.Data;

public class LexerParseData : TheoryData<string, List<ExpectedToken>>
{
    public LexerParseData()
    {
        Add
        (
            "int main(void){return 42;}",
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),
                new(TokenType.NumericConstant, "42"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int
            main
            (
            void
            )
            {
            return
            42
            ;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),
                new(TokenType.NumericConstant, "42"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            "int    main    (   void)   {   return  42   ;   }",
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),
                new(TokenType.NumericConstant, "42"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return 42;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),
                new(TokenType.NumericConstant, "42"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) 
            {
                return ~-2147483647;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),
                new(TokenType.Complement, "~"),
                new(TokenType.Minus, "-"),
                new(TokenType.NumericConstant, "2147483647"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return -5;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),                
                new(TokenType.Minus, "-"),
                new(TokenType.NumericConstant, "5"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return --5;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),                
                new(TokenType.Decrement, "--"),
                new(TokenType.NumericConstant, "5"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return 4 + 5;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),      
                new(TokenType.NumericConstant, "4"),
                new(TokenType.Plus, "+"),
                new(TokenType.NumericConstant, "5"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return 4 * 5;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),      
                new(TokenType.NumericConstant, "4"),
                new(TokenType.Asterisk, "*"),
                new(TokenType.NumericConstant, "5"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
        Add
        (
            """
            int main(void) {
                return 4 / 5;
            }
            """,
            [
                new(TokenType.Keyword, "int"),
                new(TokenType.Identifier, "main"),
                new (TokenType.OpenParenthesis, "("),
                new(TokenType.Keyword, "void"),
                new (TokenType.CloseParenthesis, ")"),
                new (TokenType.OpenBrace, "{"),
                new(TokenType.Keyword, "return"),      
                new(TokenType.NumericConstant, "4"),
                new(TokenType.ForwardSlash, "/"),
                new(TokenType.NumericConstant, "5"),
                new(TokenType.Semicolon, ";"),
                new(TokenType.CloseBrace, "}")
            ]
        );
    }
}