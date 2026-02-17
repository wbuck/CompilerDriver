using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Analysis.Test.Data.TypeChecker;
using Compiler.Analysis.Validators;
using Compiler.Common.Symbols;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;
using Xunit.Abstractions;
using static Compiler.Test.Common.Resolvers.AstTypeResolver;

namespace Compiler.Analysis.Test.Tests;

public class TypeCheckerTests(ITestOutputHelper output)
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
    [ClassData(typeof(FunctionData))]
    public void Functions(string fileContent, ProgramNode expected)
        => Assert.Equivalent(expected, GetResult(fileContent, expected), strict: true);

    [Fact]
    public void ShouldInitializeBlockScopedStaticVariableToZero()
    {
        var node = Parse
        (
            """
            int main(void) {
                static int v;
                return v;
            }                        
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v.0", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        var init = Assert.IsType<Initial<int>>(attribute.InitialValue);
        Assert.Equal(0, init.Value);
    }

    [Fact]
    public void ShouldNotMarkFunctionWithInternalLinkageAsGlobal()
    {
        var node = Parse
        (
            """
            static int func(void) {
                return 0;
            }                        
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("func", out var symbol));
        var entry = Assert.IsType<FuncEntry>(symbol);
        var attribute = Assert.IsType<FuncAttributes>(entry.Attributes);
        Assert.False(attribute.Global);
    }

    [Fact]
    public void ShouldMarkFunctionWithExternSpecifierAsGlobal()
    {
        var node = Parse
        (
            """
            extern int func(void) {
                return 0;
            }                        
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("func", out var symbol));
        var entry = Assert.IsType<FuncEntry>(symbol);
        var attribute = Assert.IsType<FuncAttributes>(entry.Attributes);
        Assert.True(attribute.Global);
    }

    [Fact]
    public void ShouldMarkFunctionWithNoSpecifierAsGlobal()
    {
        var node = Parse
        (
            """
            int func(void) {
                return 0;
            }                        
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("func", out var symbol));
        var entry = Assert.IsType<FuncEntry>(symbol);
        var attribute = Assert.IsType<FuncAttributes>(entry.Attributes);
        Assert.True(attribute.Global);
    }

    [Fact]
    public void ShouldMarkExternFileScopedVariableWithInitialValueOfNoInitializer()
    {
        var node = Parse
        (
            """
            extern int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.IsType<NoInitializer>(attribute.InitialValue);        
    }

    [Fact]
    public void ShouldMarkStaticFileScopedVariableWithInitialValueOfTentativeWhenTheVariableIsNotInitialized()
    {
        var node = Parse
        (
            """
            static int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.IsType<Tentative>(attribute.InitialValue);
    }

    [Fact]
    public void ShouldMarkFileScopedVariableWithNoSpecifierWithInitialValueOfTentativeWhenTheVariableIsNotInitialized()
    {
        var node = Parse
        (
            """
            int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.IsType<Tentative>(attribute.InitialValue);
    }

    [Fact]
    public void ShouldMarkExternFileScopedVariableAsGlobal()
    {
        var node = Parse
        (
            """
            extern int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.True(attribute.Global);
    }

    [Fact]
    public void ShouldNotMarkStaticFileScopedVariableAsGlobal()
    {
        var node = Parse
        (
            """
            static int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.False(attribute.Global);
    }

    [Fact]
    public void ShouldMarkFileScopedVariableWithNoSpecifierAsGlobal()
    {
        var node = Parse
        (
            """
            int v;                       
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.True(attribute.Global);
    }

    [Fact]
    public void ShouldMarkLocalExternVariableAsGlobal()
    {
        var node = Parse
        (
            """
            int main(void) {
                extern int v;
                return 0;
            }                     
            """
        );
        TypeChecker.Check(node);

        Assert.True(SymbolCollection.TryGetValue("v", out var symbol));
        var entry = Assert.IsType<VarEntry>(symbol);
        var attribute = Assert.IsType<StaticAttributes>(entry.Attributes);
        Assert.True(attribute.Global);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public void InvalidTypes(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            var validator = new SemanticValidator();
            var node = validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
            TypeChecker.Check(node);
        });
        Assert.Equal(message, exception.Message);
    }

    private static ProgramNode Parse(string fileContent)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var validator = new SemanticValidator();
        return validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
    }
    
    private ProgramNode GetResult(string fileContent, ProgramNode expected)
    {
        var actual = Parse(fileContent);
        TypeChecker.Check(actual);
        
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