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
        
        List<Token> tokens = [];
        IdentifierToken.Parse(input, tokens);
        
        Assert.Equal(4, tokens.Count);
        Validate(tokens[0], TokenType.Keyword, "int");
        Validate(tokens[1], TokenType.Identifier, "main");
        Validate(tokens[2], TokenType.Keyword, "void");
        Validate(tokens[3], TokenType.Keyword, "return");
        return;
        
        void Validate(Token token, TokenType expectedType,  ReadOnlySpan<char> expectedValue)
        {
            Assert.Equal(expectedValue, GetSection(input, token));
            Assert.Equal(expectedType, token.Type);
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
        
        List<Token> tokens = [];
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
        
        List<Token> tokens = [];
        CloseParenthesisToken.Parse(input, tokens);
        
        Assert.Single(tokens);
        Assert.All(tokens, token => Assert.Equal(TokenType.CloseParenthesis, token.Type));
        Assert.Equal(")", GetSection(input, tokens[0]));
    }
    
    private static ReadOnlySpan<char> GetSection(string input, Token token) 
        => input.AsSpan(token.Index, token.Length);
}