using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data.Ast;
using Compiler.Common.Tokens;
using Xunit.Abstractions;
using static Compiler.Common.Test.Data.Ast.AstTypeResolver;

namespace Compiler.Common.Test;

public class ParserTests(ITestOutputHelper output)
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
    };
    
    [Theory]
    [ClassData(typeof(InvalidParseData))]
    public void ParseWithInvalidCodeShouldFailWithExpectedMessage(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void ParsingBinaryOperationShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent));
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void ParsingBitwiseOperatorShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent));

    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void ParsingUnaryShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent));
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void ParsingLogicalAndRelationalOperatorsShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent));

    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        });
        Assert.Equal(message, exception.Message);
    }
    
    private ProgramNode GetResult(string fileContent)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var actual = ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        output.WriteLine(JsonSerializer.Serialize(actual, Options));      
        return actual;
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}