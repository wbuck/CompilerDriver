using System.Runtime.InteropServices;
using Compiler.Analysis.Annotation;
using Compiler.Analysis.Test.Data.LabelValidator;
using Compiler.Analysis.Validators;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Tests;

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
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}