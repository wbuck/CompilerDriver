using System.Collections;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;



public class TokenTests
{
    [Theory]
    [ClassData(typeof(IdentifierTokenTestData))]
    public void ParseIdentifierTokenWithValidInputsShouldSuccessfullyReturnParsedToken(int offset, string input, TokenType expectedType, string expectedValue)
    {
        var line = input.AsSpan()[offset..];
        var token = IdentifierToken.Parse(line, offset);
        
        Assert.NotNull(token);
        var idToken = Assert.IsType<IdentifierToken>(token);
        
        Assert.Equal(expectedValue, GetSection(input, idToken));
        Assert.Equal(expectedType, token.Type);
        Assert.Equal(offset, token.Index);
        Assert.Equal(expectedValue.Length, token.Length);
        Assert.Equal(idToken.Type is TokenType.Keyword ? expectedValue : null, idToken.Keyword);
    }
    
    [Theory]
    [ClassData(typeof(IntegralConstantTestData))]
    public void ParseIntegralConstantTokenWithValidInputsShouldSuccessfullyReturnParsedToken(int offset, string input, TokenType expectedType, string expectedValue)
    {
        var line = input.AsSpan()[offset..];
        var token = IntegralConstantToken.Parse(line, offset);
        
        Assert.NotNull(token);
        Assert.IsType<IntegralConstantToken>(token);
        
        Assert.Equal(expectedValue, GetSection(input, token));
        Assert.Equal(expectedType, token.Type);
        Assert.Equal(offset, token.Index);
        Assert.Equal(expectedValue.Length, token.Length);
    }
    
    [Fact]
    public void ParseCloseBraceTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = "}";
        var token = CloseBraceToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<CloseBraceToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.CloseBrace, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Fact]
    public void ParseCloseParenthesisTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = ")";
        var token = CloseParenthesisToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<CloseParenthesisToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.CloseParenthesis, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Fact]
    public void ParseCommaTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = ",";
        var token = CommaToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<CommaToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.Comma, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Fact]
    public void ParseOpenParenthesisTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = "(";
        var token = CommaToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<OpenParenthesisToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.OpenParenthesis, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Fact]
    public void ParseOpenBraceTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = "{";
        var token = OpenBraceToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<OpenBraceToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.OpenBrace, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Fact]
    public void ParseSemicolonTokenWithWithValidInputsShouldSuccessfullyReturnParsedToken()
    {
        const string input = ";";
        var token = SemicolonToken.Parse(input, 0);
        
        Assert.NotNull(token);
        Assert.IsType<SemicolonToken>(token);
        
        Assert.Equal(input, GetSection(input, token));
        Assert.Equal(TokenType.Semicolon, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData(":")]
    [InlineData(",")]
    public void ParseSemicolonTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CommaToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("}")]
    [InlineData(")")]
    public void ParseOpenBraceTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CommaToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData(")")]
    public void ParseOpenParenthesisTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CommaToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("'")]
    [InlineData(".")]
    public void ParseCommaTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CommaToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData("(")]
    public void ParseCloseParenthesisTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CloseParenthesisToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("A")]
    [InlineData("-")]
    public void ParseIntegralConstantWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = IntegralConstantToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("+, -")]
    [InlineData("@1")]
    [InlineData("1 + 2")]
    public void ParseIdentifierTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = IdentifierToken.Parse(input, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData("(")]
    public void ParseCloseBraceTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var token = CloseBraceToken.Parse(input, 0);
        Assert.Null(token);
    }

    
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