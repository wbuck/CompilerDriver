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

    private static ReadOnlySpan<char> GetSection(string input, IToken token) 
        => input.AsSpan(token.Index, token.Length);
}