using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;
using Compiler.Parser.Test.Data;
using Xunit.Abstractions;
using static Compiler.Test.Common.Resolvers.AstTypeResolver;

namespace Compiler.Parser.Test.Tests;

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
            .WithAddedModifier(AddPolymorphicTypeInfo<IBlockItem>)
            .WithAddedModifier(AddPolymorphicTypeInfo<IDeclarationNode>)
    };
    
    [Theory]
    [ClassData(typeof(InvalidParseData))]
    public void InvalidSyntax(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void BinaryOperators(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void BitwiseOperators(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void UnaryOperators(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LogicalAndRelationalData))]
    public void LogicalAndRelationalOperators(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(LocalVariableData))]
    public void LocalVariables(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(CompoundOperatorData))]
    public void CompoundOperators(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(IfStatementAndConditionalExpressionData))]
    public void IfStatementsAndConditionalExpressions(string fileContent, ProgramNode expected)
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
    public void SwitchStatements(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);
    
    [Theory]
    [ClassData(typeof(FunctionData))]
    public void Functions(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        });
        Assert.Equal(message, exception.Message);
    }
    
    private ProgramNode GetResult(string fileContent, ProgramNode expected)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var actual = ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        
        output.WriteLine("Input:");
        output.WriteLine(fileContent);
        output.WriteLine(string.Empty);
        
        output.WriteLine("Actual Result:");        
        output.WriteLine(JsonSerializer.Serialize(actual, Options));
        output.WriteLine(string.Empty);
        
        output.WriteLine("Expected Result:");        
        output.WriteLine(JsonSerializer.Serialize(expected, Options));
        output.WriteLine(string.Empty);
        
        /*
        var actualResult = JsonSerializer.Serialize(actual, Options)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var expectedResult = JsonSerializer.Serialize(expected, Options)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);



        foreach (var (line, index) in actualResult.Select((i, index) => (i, index)))
        {
            if (expectedResult.Length > index && !expectedResult[index].Equals(line))
            {
                output.WriteLine("\e[0;31m=====MISMATCH=====");
                output.WriteLine($"ACTUAL: {line}");
                output.WriteLine($"EXPECTED: {expectedResult[index]}");
                output.WriteLine("==================\e[0;37m");
                continue;
            }
            if (expectedResult.Length <= index)
            {
                output.WriteLine($"\e[0;31mEXTRA: {line}\e[0;37m");
                continue;
            }
            output.WriteLine(line);
        }
        if (expectedResult.Length > actualResult.Length)
        {
            expectedResult.Skip(actualResult.Length)
                .ToList()
                .ForEach(i => output.WriteLine($"\e[0;31mMISSING: {i}\e[0;37m"));
        }
        */
        return actual;
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}