using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.AssemblyData;
using Compiler.Common.Tokens;
using Xunit.Abstractions;

namespace Compiler.Common.Test;

public class AssemblyTests(ITestOutputHelper output)
{
    
    [Theory]
    [ClassData(typeof(ValidUnaryData))]
    public void VisitUnaryOperationsShouldSuccessfullyConvertTackyIntoAssembly(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory()), strict: true);

    [Theory]
    [ClassData(typeof(ValidBinaryOperationData))]
    public void VisitBinaryOperationsShouldSuccessfullyConvertTackyIntoAssembly(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory()), strict: true);

    private Program GetResult(ReadOnlyMemory<char> fileContent)
    {
        var result = Program.Visit(GetTacky(fileContent));
        output.WriteLine("Input:");
        output.WriteLine(fileContent.ToString());
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        result.Function.Instructions.ForEach(i => output.WriteLine(i.ToString()));
        return result;
    }
    
    private static TackyProgram GetTacky(ReadOnlyMemory<char> fileContent)
        => TackyProgram.Visit(GetAst(fileContent));
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
        => Parser.Parse(GetTokens(fileContent.Span), fileContent);
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}