using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Analysis.Annotation;
using Compiler.Analysis.Test.Data.LabelAnnotation;
using Compiler.Analysis.Validators;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;
using Xunit.Abstractions;
using static Compiler.Test.Common.Resolvers.AstTypeResolver;

namespace Compiler.Analysis.Test.Tests;

public class LabelAnnotationTests(ITestOutputHelper output)
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
    [ClassData(typeof(LoopData))]
    public void Loops(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(SwitchData))]
    public void Switch(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    private ProgramNode GetResult(string fileContent, ProgramNode expected)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var actual = ProgramNode.Parse(ref tokens, fileContent.AsMemory());

        var validator = new SemanticValidator();
        actual = validator.Validate(actual);
        
        actual = LabelAnnotation.Annotate(actual);
        
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        
        output.WriteLine("Actual Result:");        
        output.WriteLine(JsonSerializer.Serialize(actual, Options));
        output.WriteLine(string.Empty);
        
        output.WriteLine("Expected Result:");        
        output.WriteLine(JsonSerializer.Serialize(expected, Options));
        output.WriteLine(string.Empty);
        
        return actual;
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}