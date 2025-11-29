using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.Assembly;
using Compiler.Common.Tokens;
using Xunit.Abstractions;

namespace Compiler.Common.Test;

public class AssemblyTests(ITestOutputHelper output)
{
    
    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void UnaryOperators(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void BinaryOperators(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void BitwiseOperators(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void LogicalAndRelationalOperators(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    private Program GetResult(string fileContent, Program expectedResult)
    {
        var actual = Program.Visit(GetTacky(fileContent.AsMemory()));
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        
        var expected = expectedResult.Function.Instructions;
        foreach (var (instruction, index) in actual.Function.Instructions.Select((i, index) => (i, index)))
        {            
            if (expected.Count > index && !expected[index].Equals(instruction))
            {
                output.WriteLine("\e[0;31m=====MISMATCH=====");
                output.WriteLine($"ACTUAL: {instruction}");
                output.WriteLine($"EXPECTED: {expected[index]}");
                output.WriteLine("==================\e[0;37m");
                continue;
            }
            if (expected.Count <= index)
            {
                output.WriteLine($"\e[0;31mEXTRA: {instruction}\e[0;37m");
                continue;           
            }
            output.WriteLine(instruction.ToString());
        }
        if (expected.Count > actual.Function.Instructions.Count)
        {
            expected.Skip(actual.Function.Instructions.Count)
                .ToList()
                .ForEach(i => output.WriteLine($"\e[0;31mMISSING: {i}\e[0;37m"));           
        }
        
        return actual;
    }
    
    private static TackyProgram GetTacky(ReadOnlyMemory<char> fileContent)
        => new TackyVisitor().Visit(GetAst(fileContent));
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
    {
        SemanticValidator validator = new();
        return validator.Validate(Parser.Parse(GetTokens(fileContent.Span), fileContent));
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}