using Compiler.Common.Stages;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class TokenTests
{
    [Fact]
    public void ParseIdentifierToken()
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
        IdentifierToken.Parse(input, tokens);
        
        Assert.Equal(14, tokens.Count);
        Validate(tokens[0], TokenType.Keyword, "void");
        Validate(tokens[1], TokenType.Identifier, "add");
        Validate(tokens[2], TokenType.Keyword, "int");
        Validate(tokens[3], TokenType.Identifier, "a");
        Validate(tokens[4], TokenType.Keyword, "int");
        Validate(tokens[5], TokenType.Identifier, "b");
        Validate(tokens[6], TokenType.Keyword, "return");
        Validate(tokens[7], TokenType.Identifier, "a");
        Validate(tokens[8], TokenType.Identifier, "b");        
        Validate(tokens[9], TokenType.Keyword, "int");
        Validate(tokens[10], TokenType.Identifier, "main");
        Validate(tokens[11], TokenType.Keyword, "void");
        Validate(tokens[12], TokenType.Keyword, "return");
        Validate(tokens[13], TokenType.Identifier, "add");
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

    [Fact]
    public void LexerTest()
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
        
        var tokens = Lexer.Lex(input);
        Assert.Equal(30, tokens.Count);
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
        Validate(tokens[12], TokenType.Identifier, "b");
        Validate(tokens[13], TokenType.Semicolon, ";");
        Validate(tokens[14], TokenType.CloseBrace, "}");
        Validate(tokens[15], TokenType.Keyword, "int");
        Validate(tokens[16], TokenType.Identifier, "main");
        Validate(tokens[17], TokenType.OpenParenthesis, "(");        
        Validate(tokens[18], TokenType.Keyword, "void");
        Validate(tokens[19], TokenType.CloseParenthesis, ")");
        Validate(tokens[20], TokenType.OpenBrace, "{");
        Validate(tokens[21], TokenType.Keyword, "return");
        Validate(tokens[22], TokenType.Identifier, "add");
        Validate(tokens[23], TokenType.OpenParenthesis, "(");
        Validate(tokens[24], TokenType.IntegralConstant, "1");
        Validate(tokens[25], TokenType.Comma, ",");
        Validate(tokens[26], TokenType.IntegralConstant, "2");
        Validate(tokens[27], TokenType.CloseParenthesis, ")");
        Validate(tokens[28], TokenType.Semicolon, ";");
        Validate(tokens[29], TokenType.CloseBrace, "}");
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