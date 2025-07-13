using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data.LabelValidator;
using Compiler.Common.Tokens;
using Xunit.Abstractions;
using static Compiler.Common.Test.Data.Ast.AstTypeResolver;

namespace Compiler.Common.Test;

public class LabelValidatorTests(ITestOutputHelper output)
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
    [ClassData(typeof(InvalidLabelData))]
    public void ParseWithInvalidLabelDataShouldFailWithExpectedMessage(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            var validator = new LabelValidator();
            validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
        });
        Assert.Equal(message, exception.Message);
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}