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
    public void ParsingUnaryShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void ParsingBinaryOperatorShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void ParsingBitwiseOperatorShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void ParsingLogicalAndRelationalOperatorsShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void ParsingLocalVariableDataShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(CompoundOperatorData))]
    public void ParsingCompoundOperatorDataShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);
    
    [Theory]
    [ClassData(typeof(IfStatementAndConditionalExpressionData))]
    public void ParsingIfStatementAndConditionalExpressionDataShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, GetResult(fileContent), true);

    private TackyProgram GetResult(string fileContent)
    {
        SemanticValidator validator = new();
        var ast = validator.Validate(GetAst(fileContent.AsMemory()));
        
        var actual = new TackyVisitor().Visit(ast);
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