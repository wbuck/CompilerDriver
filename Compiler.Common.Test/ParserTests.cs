using Compiler.Common.Stages;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class ParserTests
{
    private const string FileContent = """
        void add(int a, int b)
        {
            return a + b;
        }
        int main(void) 
        {
           return add(1, 2);
        }
        """;
    
    [Fact]
    public void Parse()
    {
        var tokens = GetTokens();
        var parser = new Parser(FileContent.AsMemory());
        
        parser.Parse(tokens);
    }

    private static List<IToken> GetTokens()
    {
        Assert.True(Lexer.TryTokenize(FileContent, out var tokens));
        return tokens;
    }
}