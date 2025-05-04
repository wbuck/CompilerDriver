using System.Runtime.InteropServices;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data.NodeData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class ParserTests
{
    [Theory]
    [ClassData(typeof(InvalidParseData))]
    public void ParseWithInvalidCodeShouldFailWithExpectedMessage(string fileContent, string message)
        => InvalidCheck(fileContent, message);
    
    
    [Theory]
    [ClassData(typeof(ValidBinaryOperationData))]
    public void ParsingBinaryOperationShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => ValidCheck(fileContent, expected);

    [Theory]
    [ClassData(typeof(ValidUnaryData))]
    public void ParsingUnaryShouldSuccessfullyConvertTokensInToAst(string fileContent, ProgramNode expected)
        => ValidCheck(fileContent, expected);

    private static void InvalidCheck(string fileContent, string message)
    {        
        var exception = Assert.Throws<FormatException>(() =>
        {
            var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
            ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        });
        Assert.Equal(message, exception.Message);
    }

    private static void ValidCheck(string fileContent, ProgramNode expected)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var actual = ProgramNode.Parse(ref tokens, fileContent.AsMemory());
        Assert.Equivalent(expected, actual);
    }

    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}