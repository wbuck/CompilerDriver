using System.Diagnostics;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data;
using Compiler.Common.Test.Data.NodeData;
using Compiler.Common.Test.Data.TackyData;
using Compiler.Common.Tokens;
using ValidBinaryOperationData = Compiler.Common.Test.Data.TackyData.ValidBinaryOperationData;
using ValidUnaryData = Compiler.Common.Test.Data.TackyData.ValidUnaryData;

namespace Compiler.Common.Test;

public class TackyTests
{
    [Theory]
    [ClassData(typeof(ValidUnaryData))]
    public void ParsingUnaryShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, TackyProgram.Visit(GetAst(fileContent.AsMemory())), true);
    
    [Theory]
    [ClassData(typeof(ValidBinaryOperationData))]
    public void ParsingBinaryOperationShouldSuccessfullyConvertAstInToTacky(string fileContent, TackyProgram expected)
        => Assert.Equivalent(expected, TackyProgram.Visit(GetAst(fileContent.AsMemory())), true);
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
        => Parser.Parse(GetTokens(fileContent.Span), fileContent);
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}