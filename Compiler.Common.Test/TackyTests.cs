using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.Tacky;
using Compiler.Common.Tokens;
using Xunit.Abstractions;
using static Compiler.Common.Test.Data.Tacky.TackyTypeResolver;

namespace Compiler.Common.Test;

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
    };
    
    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void UnaryOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void BinaryOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void BitwiseOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void LogicalAndRelationalOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(CompoundOperatorData))]
    public void CompoundOperators(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(IfStatementAndConditionalExpressionData))]
    public void IfStatementsAndConditionalExpressions(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(CompoundStatementData))]
    public void CompoundStatements(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(LoopData))]
    public void Loops(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(SwitchData))]
    public void Switch(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);

    private TackyProgram GetResult(string fileContent)
    {
        SemanticValidator validator = new();
        var node = validator.Validate(GetAst(fileContent.AsMemory()));
        node = LabelAnnotation.Annotate(node);
        
        var actual = new TackyVisitor().Visit(node);
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        output.WriteLine(JsonSerializer.Serialize(actual, Options)); 
        return actual;
    }
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
        => Parser.Parse(GetTokens(fileContent.Span), fileContent);
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}