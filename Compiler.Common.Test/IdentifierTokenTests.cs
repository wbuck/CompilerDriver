using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class IdentifierTokenTests
{
    [Fact]
    public void Parse()
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
        return;

        static ReadOnlySpan<char> GetSection(string input, Token token) 
            => input.AsSpan(token.Index, token.Length);
    }
}