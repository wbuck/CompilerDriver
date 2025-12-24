using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Analysis.Test.Data.SemanticValidator;
using Compiler.Analysis.Validators;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;
using Xunit.Abstractions;
using static Compiler.Test.Common.Resolvers.AstTypeResolver;

namespace Compiler.Analysis.Test.Tests;

public class SemanticValidatorTests(ITestOutputHelper output)
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            .WithAddedModifier(AddPolymorphicTypeInfo<IStatementNode>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IExpressionNode>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IUnaryOperatorNode>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IBinaryOperatorNode>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IBitwiseOperatorNode>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IBlockItem>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IDeclarationNode>)
    };
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(CompoundStatementData))]
    public void CompoundStatements(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LoopData))]
    public void Loops(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(SwitchData))]
    public void Switch(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(FunctionData))]
    public void Functions(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(InvalidSemanticData))]
    public void InvalidSemantics(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            var validator = new SemanticValidator();
            validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
        });
        Assert.Equal(message, exception.Message);
    }
    
    private ProgramNode GetResult(string fileContent, ProgramNode expected)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var validator = new SemanticValidator();
        var actual = validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
        
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

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}