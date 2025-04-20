using Compiler.Common.Tokens;

namespace Compiler.Common.Test.Data.TokenData;

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
                new(TokenType.BitwiseComplement, "~"),
                new(TokenType.Negation, "-"),
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
                new(TokenType.Negation, "-"),
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
    }
}