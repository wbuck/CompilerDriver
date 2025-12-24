using Compiler.Analysis.Annotation;
using Compiler.Analysis.Validators;
using Compiler.Emission.Test.Data;
using Compiler.Generation.Instructions;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser;
using Compiler.Parser.Nodes;
using Compiler.Tacky;
using Compiler.Tacky.Tac;
using Xunit.Abstractions;

namespace Compiler.Emission.Test.Tests;

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
    
    [Theory]
    [ClassData(typeof(FunctionData))]
    public void Functions(string fileContent, string[] expected)
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
        
        output.WriteLine(string.Empty);
        output.WriteLine("Expected Result:"); 
        foreach (var line in expected) output.WriteLine(line);
        
        return actual;
    }
    
    private static Program GetAssembly(ReadOnlyMemory<char> fileContent)
        => Program.Visit(GetTacky(fileContent));
    
    private static TackyProgram GetTacky(ReadOnlyMemory<char> fileContent)
        => new TackyVisitor().Visit(GetAst(fileContent));
   
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
    {
        SemanticValidator validator = new();
        var node = validator.Validate(TokenParser.Parse(GetTokens(fileContent.Span), fileContent));
        
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