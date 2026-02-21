using Compiler.Analysis.Annotation;
using Compiler.Analysis.Validators;
using Compiler.Generation.Instructions;
using Compiler.Generation.Test.Data;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser;
using Compiler.Parser.Nodes;
using Compiler.Tacky;
using Compiler.Tacky.Tac;
using Xunit.Abstractions;

namespace Compiler.Generation.Test.Tests;

public class GeneratorTests(ITestOutputHelper output)
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
    
    [Theory]
    [ClassData(typeof(FunctionData))]
    public void Function(string fileContent, Program expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    private Program GetResult(string fileContent, Program expectedResult)
    {        
        var actual = Program.Visit(GetTacky(fileContent.AsMemory()));
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        actual.TopLevel.ForEach(f =>
        {
            output.WriteLine("Function {");
            output.WriteLine($"  Name = {f.Name},");
            output.WriteLine("  Instructions = [");
            f.Instructions.ForEach(i => output.WriteLine($"    {i}"));
            output.WriteLine("  ]");
            output.WriteLine("}");
        });
        
        output.WriteLine(string.Empty);
        output.WriteLine("Expected Result:");
        expectedResult.TopLevel.ForEach(f =>
        {
            output.WriteLine("Function {");
            output.WriteLine($"  Name = {f.Name},");
            output.WriteLine("  Instructions = [");
            f.Instructions.ForEach(i => output.WriteLine($"    {i}"));
            output.WriteLine("  ]");
            output.WriteLine("}");
        });
        
        return actual;
    }
    
    private static TackyProgram GetTacky(ReadOnlyMemory<char> fileContent)
        => new TackyVisitor().Visit(GetAst(fileContent));
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
    {
        var node = TokenParser.Parse(GetTokens(fileContent.Span), fileContent);
        node = new SemanticValidator().Validate(node);
        
        TypeChecker.Check(node);        
        node = LabelAnnotation.Annotate(node);
        
        Assert.True(LabelValidator.TryValidate(node));
        
        node = new LabelReplacer().Replace(node);
        return node;
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}