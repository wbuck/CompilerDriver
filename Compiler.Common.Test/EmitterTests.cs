using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.Emitter;
using Compiler.Common.Tokens;
using Xunit.Abstractions;

namespace Compiler.Common.Test;

public class EmitterTests(ITestOutputHelper output)
{    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void BinaryOperators(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);

    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void UnaryOperators(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);

    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void BitwiseOperators(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void LogicalAndRelationalOperators(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);    
    
    [Theory]
    [ClassData(typeof(CompoundStatementData))]
    public void CompoundStatements(string fileContent, string[] expected)
        => Assert.Equivalent(expected, GetResult(fileContent.AsMemory(), expected), strict: true);
    
    private List<string> GetResult(ReadOnlyMemory<char> fileContent, string[] expected)
    {
        var compiled = Emitter.Emit(GetAssembly(fileContent));
        var actual = compiled
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .ToList();
        
        output.WriteLine("Input:");
        output.WriteLine(fileContent.ToString());
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        
        actual.ForEach(output.WriteLine);
        
        // foreach (var (instruction, index) in actual.Select((i, index) => (i, index)))
        // {            
        //     if (expected.Length > index && !expected[index].Equals(instruction))
        //     {
        //         output.WriteLine("\e[0;31m=====MISMATCH=====");
        //         output.WriteLine($"ACTUAL: {instruction}");
        //         output.WriteLine($"EXPECTED: {expected[index]}");
        //         output.WriteLine("==================\e[0;37m");
        //         continue;
        //     }
        //     if (expected.Length <= index)
        //     {
        //         output.WriteLine($"\e[0;31mEXTRA: {instruction}\e[0;37m");
        //         continue;           
        //     }
        //     output.WriteLine(instruction);
        // }
        // if (expected.Length > actual.Count)
        // {
        //     expected.Skip(actual.Count)
        //         .ToList()
        //         .ForEach(i => output.WriteLine($"\e[0;31mMISSING: {i}\e[0;37m"));           
        // }       
        return actual;
    }
    
    private static Program GetAssembly(ReadOnlyMemory<char> fileContent)
        => Program.Visit(GetTacky(fileContent));
    
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