using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Analysis.Annotation;
using Compiler.Analysis.Validators;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser;
using Compiler.Parser.Nodes;
using Compiler.Tacky.Tac;
using Compiler.Tacky.Test.Data;
using Xunit.Abstractions;
using static Compiler.Test.Common.Resolvers.TackyTypeResolver;

namespace Compiler.Tacky.Test.Tests;

public class TackyTests(ITestOutputHelper output)
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyInstruction>)
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyValue>)
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyUnaryOperator>)
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyBinaryOperator>)
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyBitwiseOperator>)
            .WithAddedModifier(AddPolymorphicTypeInfo<ITackyTopLevel>)
    };
    
    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void UnaryOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void BinaryOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void BitwiseOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void LogicalAndRelationalOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(CompoundOperatorData))]
    public void CompoundOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(IfStatementAndConditionalExpressionData))]
    public void IfStatementsAndConditionalExpressions(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(CompoundStatementData))]
    public void CompoundStatements(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(LoopData))]
    public void Loops(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    [Theory]
    [ClassData(typeof(SwitchData))]
    public void Switch(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);
    
    
    [Theory]
    [ClassData(typeof(FunctionData))]
    public void Function(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), true);

    private TackyProgram GetResult(string fileContent, TackyProgram expected)
    {
        SemanticValidator validator = new();
        var node = validator.Validate(GetAst(fileContent.AsMemory()));
        TypeChecker.Check(node);
        node = LabelAnnotation.Annotate(node);
        Assert.True(LabelValidator.TryValidate(node));
        node = new LabelReplacer().Replace(node);
        
        var actual = new TackyVisitor().Visit(node);
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        output.WriteLine(JsonSerializer.Serialize(actual, Options)); 
        
        output.WriteLine(string.Empty);
        output.WriteLine("Expected Result:");
        output.WriteLine(JsonSerializer.Serialize(expected, Options));
        return actual;
    }
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
        => TokenParser.Parse(GetTokens(fileContent.Span), fileContent);
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}