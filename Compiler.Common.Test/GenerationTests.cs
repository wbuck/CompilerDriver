using System.Diagnostics;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class GenerationTests
{
    [Fact]
    public void Test()
    {
        const string fileContent = """
            int main(void) 
            {
               return 2;
            }
            """;
        
        var tokens = GetTokens(fileContent);
        var ast = Parser.Parse(tokens, fileContent.AsMemory());
        var parsed = Generator.Visit(ast);
        
        var program = parsed.Build();
        Debugger.Break();
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}