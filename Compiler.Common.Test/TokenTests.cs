using Compiler.Common.Stages;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class TokenTests
{    
    [Fact]
    public void LexerShouldSuccessfullyTokenizeInput()
    {
        const string input = """
            void add(int a, int b)
            {
                return a + b;
            }
            int main(void) 
            {
               return add(1, 2);
            }
            """;
        
        Assert.True(Lexer.TryTokenize(input, out var tokens));
        
        Assert.Equal(31, tokens.Count);
        Validate(tokens[0], TokenType.Keyword, "void");
        Validate(tokens[1], TokenType.Identifier, "add");
        Validate(tokens[2], TokenType.OpenParenthesis, "(");
        Validate(tokens[3], TokenType.Keyword, "int");
        Validate(tokens[4], TokenType.Identifier, "a");
        Validate(tokens[5], TokenType.Comma, ",");               
        Validate(tokens[6], TokenType.Keyword, "int");
        Validate(tokens[7], TokenType.Identifier, "b");
        Validate(tokens[8], TokenType.CloseParenthesis, ")");
        Validate(tokens[9], TokenType.OpenBrace, "{");
        Validate(tokens[10], TokenType.Keyword, "return");
        Validate(tokens[11], TokenType.Identifier, "a");
        Validate(tokens[12], TokenType.Plus, "+");
        Validate(tokens[13], TokenType.Identifier, "b");
        Validate(tokens[14], TokenType.Semicolon, ";");
        Validate(tokens[15], TokenType.CloseBrace, "}");
        Validate(tokens[16], TokenType.Keyword, "int");
        Validate(tokens[17], TokenType.Identifier, "main");
        Validate(tokens[18], TokenType.OpenParenthesis, "(");        
        Validate(tokens[19], TokenType.Keyword, "void");
        Validate(tokens[20], TokenType.CloseParenthesis, ")");
        Validate(tokens[21], TokenType.OpenBrace, "{");
        Validate(tokens[22], TokenType.Keyword, "return");
        Validate(tokens[23], TokenType.Identifier, "add");
        Validate(tokens[24], TokenType.OpenParenthesis, "(");
        Validate(tokens[25], TokenType.IntegralConstant, "1");
        Validate(tokens[26], TokenType.Comma, ",");
        Validate(tokens[27], TokenType.IntegralConstant, "2");
        Validate(tokens[28], TokenType.CloseParenthesis, ")");
        Validate(tokens[29], TokenType.Semicolon, ";");
        Validate(tokens[30], TokenType.CloseBrace, "}");
        return;
        
        void Validate(IToken token, TokenType expectedType,  ReadOnlySpan<char> expectedValue)
        {
            Assert.Equal(expectedValue, GetSection(input, token));
            Assert.Equal(expectedType, token.Type);
            if (token is IdentifierToken { Type: TokenType.Keyword } keywordToken)
            {
                Assert.Equal(expectedValue, keywordToken.Keyword);
            }
        }
    }
    
    private static ReadOnlySpan<char> GetSection(string input, IToken token) 
        => input.AsSpan(token.Index, token.Length);
}