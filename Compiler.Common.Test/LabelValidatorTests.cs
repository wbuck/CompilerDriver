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

public class LabelValidatorTests
{
    [Theory]
    [ClassData(typeof(InvalidLabelData))]
    public void ParseWithInvalidLabelDataShouldFailWithExpectedMessage(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            var node = ProgramNode.Parse(ref tokens, fileContent.AsMemory());
            
            node = new SemanticValidator().Validate(node);
            node = LabelAnnotation.Annotate(node);
            
            var validator = new LabelValidator();
            validator.Validate(node);
        });
        Assert.Equal(message, exception.Message);
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}