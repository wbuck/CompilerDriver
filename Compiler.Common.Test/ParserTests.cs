using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class ParserTests
{    
    [Theory]
    [ClassData(typeof(ParseSimpleMainData))]
    public void ParseWithSimpleMainFunctionShouldSuccessfullyProduceAst(string fileContent, int expectedConstant)
    {
        var tokens = GetTokens(fileContent);
        var actual = Parser.Parse(tokens, fileContent.AsMemory());
        
        var programNode = Assert.IsType<ProgramNode>(actual);
        Assert.Single(programNode.Nodes);
        
        var functionNode = Assert.IsType<FunctionNode>(programNode.Nodes[0]);
        Assert.Equal("main".AsMemory(), functionNode.Name);
        Assert.Equal("int".AsMemory(), functionNode.ReturnType);
        Assert.Null(functionNode.Arguments);
        
        var blockNode = Assert.IsType<BlockStatementNode>(functionNode.Body);
        Assert.Single(blockNode.Body);
        
        var returnNode = Assert.IsType<ReturnNode>(blockNode.Body[0]);
        Assert.NotNull(returnNode.Expression);
        
        var constantNode = Assert.IsType<IntegerConstantNode>(returnNode.Expression);
        Assert.Equal(expectedConstant, constantNode.Value);
    }
    
    [Theory]
    [ClassData(typeof(InvalidParseData))]
    public void ParseWithInvalidCodeShouldFailWithExpectedMessage(string fileContent, string message)
    {
        var tokens = GetTokens(fileContent);
        var exceptions = Assert.Throws<FormatException>(() => Parser.Parse(tokens, fileContent.AsMemory()));
        Assert.Equal(message, exceptions.Message);
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}