using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class TokenTests
{
    [Fact]
    public void ParseIdentifierToken()
    {
        const string input = """
            int main(void) 
            {
                return 2;
            }
            """;
        
        List<IToken> tokens = [];
        IdentifierToken.Parse(input, tokens);
        
        Assert.Equal(4, tokens.Count);
        Validate(tokens[0], TokenType.Keyword, "int");
        Validate(tokens[1], TokenType.Identifier, "main");
        Validate(tokens[2], TokenType.Keyword, "void");
        Validate(tokens[3], TokenType.Keyword, "return");
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
    
    [Fact]
    public void ParseOpenParenthesisToken()
    {
        const string input = """
                             int main(void) 
                             {
                                 return 2;
                             }
                             """;
        
        List<IToken> tokens = [];
        OpenParenthesisToken.Parse(input, tokens);
        
        Assert.Single(tokens);
        Assert.All(tokens, token => Assert.Equal(TokenType.OpenParenthesis, token.Type));
        Assert.Equal("(", GetSection(input, tokens[0]));
    }
    
    [Fact]
    public void CloseOpenParenthesisToken()
    {
        const string input = """
                             int main(void) 
                             {
                                 return 2;
                             }
                             """;
        
        List<IToken> tokens = [];
        CloseParenthesisToken.Parse(input, tokens);
        
        Assert.Single(tokens);
        Assert.All(tokens, token => Assert.Equal(TokenType.CloseParenthesis, token.Type));
        Assert.Equal(")", GetSection(input, tokens[0]));
    }
    
    [Fact]
    public void ParseCommaToken()
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
        
        List<IToken> tokens = [];
        CommaToken.Parse(input, tokens);
        
        Assert.Equal(2, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(TokenType.Comma, token.Type));
        Assert.Equal(",", GetSection(input, tokens[0]));
        Assert.Equal(",", GetSection(input, tokens[1]));
    }
    
    [Fact]
    public void ParseIntegralConstantToken()
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
        
        List<IToken> tokens = [];
        IntegralConstantToken.Parse(input, tokens);
        
        Assert.Equal(2, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(TokenType.IntegralConstant, token.Type));
        Assert.Equal("1", GetSection(input, tokens[0]));
        Assert.Equal("2", GetSection(input, tokens[1]));
    }
    
    [Fact]
    public void ParseCloseBraceToken()
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
        
        List<IToken> tokens = [];
        CloseBraceToken.Parse(input, tokens);
        
        Assert.Equal(2, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(TokenType.CloseBrace, token.Type));
        Assert.Equal("}", GetSection(input, tokens[0]));
        Assert.Equal("}", GetSection(input, tokens[1]));
    }
    
    [Fact]
    public void ParseOpenBraceToken()
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
        
        List<IToken> tokens = [];
        OpenBraceToken.Parse(input, tokens);
        
        Assert.Equal(2, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(TokenType.OpenBrace, token.Type));
        Assert.Equal("{", GetSection(input, tokens[0]));
        Assert.Equal("{", GetSection(input, tokens[1]));
    }
    
    [Fact]
    public void ParseSemiColonToken()
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
        
        List<IToken> tokens = [];
        SemiColonToken.Parse(input, tokens);
        
        Assert.Equal(2, tokens.Count);
        Assert.All(tokens, token => Assert.Equal(TokenType.Semicolon, token.Type));
        Assert.Equal(";", GetSection(input, tokens[0]));
        Assert.Equal(";", GetSection(input, tokens[1]));
    }
    
    private static ReadOnlySpan<char> GetSection(string input, IToken token) 
        => input.AsSpan(token.Index, token.Length);
}