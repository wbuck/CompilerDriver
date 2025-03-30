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
        Assert.All(tokens, token => Assert.Equal(TokenType.Identifier, token.Type));
        Assert.Equal("int", GetSection(input, tokens[0]));
        Assert.Equal("main", GetSection(input, tokens[1]));
        Assert.Equal("void", GetSection(input, tokens[2]));
        Assert.Equal("return", GetSection(input, tokens[3]));
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