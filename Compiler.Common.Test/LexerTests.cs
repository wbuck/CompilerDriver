using Compiler.Common.Stages;
using Compiler.Common.Test.Data;
using Compiler.Common.Test.Data.TokenData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class LexerTests
{
    [Theory]
    [ClassData(typeof(LexerParseData))]
    public void LexerShouldSuccessfullyTokenizeInput(string input, List<ExpectedToken> expectedTokens)
    {
        Assert.True(Lexer.TryTokenize(input, out var tokens));
        Assert.Equal(expectedTokens.Count, tokens.Count);

        foreach (var (token, (type, value)) in tokens.Zip(expectedTokens))
            Validate(token, type, value);

        return;
        
        void Validate(IToken token, TokenType expectedType,  ReadOnlySpan<char> expectedValue)
        {
            Assert.Equal(expectedValue, GetSection(input, token));
            Assert.Equal(expectedType, token.Type);
            
            if (token is KeywordToken keywordToken)
                Assert.Equal(expectedValue, keywordToken.Keyword);
        }
    }
    
    private static ReadOnlySpan<char> GetSection(string input, IToken token) 
        => input.AsSpan(token.Index, token.Length);
}