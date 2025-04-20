using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data;
using Compiler.Common.Test.Data.NodeData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class ParserTests
{        
    [Theory]
    [ClassData(typeof(InvalidParseData))]
    public void ParseWithInvalidCodeShouldFailWithExpectedMessage(string fileContent, string message)
    {
        var tokens = GetTokens(fileContent);
        var exceptions = Assert.Throws<FormatException>(() => Parser.Parse(tokens, fileContent.AsMemory()));
        Assert.Equal(message, exceptions.Message);
    }


    [Theory]
    [ClassData(typeof(ValidParseData))]
    public void ParseWithValidDataShouldSuccessfullyConvertTokensInToAst(string fileContent, ExpectedParseResultBase expected)
    {
        var tokens = GetTokens(fileContent);
        var actual = Parser.Parse(tokens, fileContent.AsMemory());
        expected.Verify(actual);
    }
    
    
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}