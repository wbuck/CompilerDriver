using System.Reflection;
using Compiler.Common.Test.Data.TokenData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;



public class TokenTests
{
    private static readonly MethodInfo ParseValidMethod = typeof(TokenTests)
        .GetMethod(nameof(ParseValid), BindingFlags.NonPublic | BindingFlags.Static)!;
    
    
    [Theory]
    [ClassData(typeof(SingleCharacterValidTokenData))]
    public void ParseSingleCharacterTokenShouldSuccessfullyReturnParsedToken(
        string input, Type tokenType, TokenType expectedType)
    {
        ParseValidMethod.MakeGenericMethod(tokenType)
            .Invoke(null, [input, input, expectedType]);
    }
    
    [Theory]
    [ClassData(typeof(IdentifierTokenTestData))]
    public void ParseIdentifierTokenWithValidInputsShouldSuccessfullyReturnParsedToken(int offset, string input, TokenType expectedType, string expectedValue)
    {
        var line = input.AsSpan()[offset..];
        var token = IdentifierToken.Parse(ref line, offset);
        
        Assert.NotNull(token);

        switch (token.Type)
        {
            case TokenType.Keyword:
                Assert.IsType<KeywordToken>(token);
                break;
            case TokenType.Identifier:
                Assert.IsType<IdentifierToken>(token);
                break;
            default:
                Assert.Fail("Unexpected token type.");
                break;
        }

        Assert.Equal(expectedValue, GetSection(input, token));
        Assert.Equal(expectedType, token.Type);
        Assert.Equal(offset, token.Index);
        Assert.Equal(expectedValue.Length, token.Length);
        
        if (token is KeywordToken keywordToken)
            Assert.Equal(expectedValue, keywordToken.Keyword);
    }
    
    [Theory]
    [ClassData(typeof(NumericConstantTestData))]
    public void ParseNumericConstantTokenWithValidInputsShouldSuccessfullyReturnParsedToken(int offset, string input, TokenType expectedType, string expectedValue)
    {
        var line = input.AsSpan()[offset..];
        var token = NumericConstantToken.Parse(ref line, offset);
        
        Assert.NotNull(token);
        Assert.IsType<NumericConstantToken>(token);
        
        Assert.Equal(expectedValue, GetSection(input, token));
        Assert.Equal(expectedType, token.Type);
        Assert.Equal(offset, token.Index);
        Assert.Equal(expectedValue.Length, token.Length);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData(":")]
    [InlineData(",")]
    public void ParseSemicolonTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = SemicolonToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("}")]
    [InlineData(")")]
    public void ParseOpenBraceTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = OpenBraceToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData(")")]
    public void ParseOpenParenthesisTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = OpenParenthesisToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("'")]
    [InlineData(".")]
    public void ParseCommaTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = CommaToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData("(")]
    public void ParseCloseParenthesisTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = CloseParenthesisToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("A")]
    [InlineData("-")]
    [InlineData("+")]
    [InlineData("--.23")]
    [InlineData("++23")]
    [InlineData("+--0.23")]
    public void ParseNumericConstantWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = NumericConstantToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("+, -")]
    [InlineData("@1")]
    [InlineData("1 + 2")]
    public void ParseIdentifierTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = IdentifierToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("@")]
    [InlineData("{")]
    [InlineData("(")]
    public void ParseCloseBraceTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = CloseBraceToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("++")]
    [InlineData("-")]
    [InlineData("_")]
    public void ParseDecrementTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = DecrementToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("++")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParseMinusTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = MinusToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("'")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParseBitwiseComplementTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = BitwiseComplementToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("'")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParsePlusTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = PlusToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("'")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParseAsteriskTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = AsteriskToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("'")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParsePercentTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = PercentToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    [Theory]
    [InlineData("'")]
    [InlineData("=")]
    [InlineData("_")]
    public void ParseForwardSlashTokenWithUnrecognizedInputShouldReturnNull(string input)
    {
        var line = input.AsSpan();
        var token = ForwardSlashToken.Parse(ref line, 0);
        Assert.Null(token);
    }
    
    private static void ParseValid<TToken>(string input, string expectedValue, TokenType expectedType) 
        where TToken: IToken
    {
        var copy = input.AsSpan();
        var token = TToken.Parse(ref copy, 0);
        
        Assert.NotNull(token);
        Assert.IsType<TToken>(token);
        
        Assert.Equal(expectedValue, GetSection(input, token));
        Assert.Equal(expectedType, token.Type);
        Assert.Equal(0, token.Index);
        Assert.Equal(input.Length, token.Length);
    }

    private static ReadOnlySpan<char> GetSection(ReadOnlySpan<char> input, IToken token) 
        => input.Slice(token.Index, token.Length);
}