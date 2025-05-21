using Compiler.Common.Stages;
using Compiler.Common.Test.Data;
using Compiler.Common.Test.Data.TokenData;
using Compiler.Common.Tokens;
using Xunit.Abstractions;

namespace Compiler.Common.Test;

public class LexerTests(ITestOutputHelper output)
{
    [Theory]
    [ClassData(typeof(LexerParseData))]
    public void LexerShouldSuccessfullyTokenizeInput(string fileContent, List<ExpectedToken> expected)
        => Validate(fileContent, expected, GetResult(fileContent));
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void ShouldSuccessfullyHandleBitwiseOperatorsData(string fileContent, List<ExpectedToken> expected)
        => Validate(fileContent, expected, GetResult(fileContent)); 
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void ShouldSuccessfullyHandleLogicalAndRelationalOperatorsData(string fileContent, List<ExpectedToken> expected)
        => Validate(fileContent, expected, GetResult(fileContent)); 
    
    private static void Validate(string fileContent, List<ExpectedToken> expected, List<IToken> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        
        foreach (var (token, (type, value)) in actual.Zip(expected))
            ValidateToken(token, type, value);
        
        return;

        void ValidateToken(IToken token, TokenType expectedType, ReadOnlySpan<char> expectedValue)
        {
            Assert.Equal(expectedValue, GetSection(fileContent, token));
            Assert.Equal(expectedType, token.Type);
            
            if (token is KeywordToken keywordToken)
                Assert.Equal(expectedValue, keywordToken.Keyword);
        }
    }
    
    private static ReadOnlySpan<char> GetSection(string input, IToken token) 
        => input.AsSpan(token.Index, token.Length);
    
    private List<IToken> GetResult(string fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        tokens.ForEach(i => output.WriteLine(i.ToString()));
        return tokens;
    }
}