using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.AssemblyData;
using Compiler.Common.Test.Data.TackyData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class GenerationTests
{
    [Theory]
    [ClassData(typeof(AssemblyValidData))]
    public void VisitShouldSuccessfullyConvertInput(string fileContent, Program expected)
    {
        var node = GetAst(fileContent.AsMemory());
        Assert.True(Generator.TryGenerate(node, out var program));
        Assert.Equivalent(expected, program, strict: true);
    }

    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
    {
        var tokens = GetTokens(fileContent.Span);
        return Parser.Parse(tokens, fileContent);
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}