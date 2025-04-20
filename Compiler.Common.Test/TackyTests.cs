using System.Diagnostics;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data;
using Compiler.Common.Test.Data.NodeData;
using Compiler.Common.Test.Data.TackyData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class TackyTests
{
    [Theory]
    [ClassData(typeof(TackyValidData))]
    public void VisitShouldSuccessfullyConvertAstToTackyAst(string fileContent, TackyProgram expected)
    {
        var node = GetAst(fileContent.AsMemory());
        Assert.Equivalent(expected, TackyBase.Visit(node), true);
    }

    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
    {
        var tokens = GetTokens(fileContent.Span);
        return Parser.Parse(tokens, fileContent);
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}