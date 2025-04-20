using System.Diagnostics;
using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Test.Data.AssemblyData;
using Compiler.Common.Test.Data.EmitterData;
using Compiler.Common.Tokens;

namespace Compiler.Common.Test;

public class EmitterTests
{
    [Theory]
    [ClassData(typeof(EmitterValidData))]
    public void EmitShouldCorrectCompileAst(string fileContent, string[] expected)
    {
        var compiled = Emitter.Emit(GetProgram(fileContent.AsMemory()));
        var actual = compiled
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .ToArray();
        
        Assert.Equal(expected.Length, actual.Length);
        
        foreach (var (expectedLine, actualLine) in expected.Zip(actual))
            Assert.Equal(expectedLine, actualLine);
    }

    private static Program GetProgram(ReadOnlyMemory<char> fileContent)
        => Generator.Generate(GetAst(fileContent));
    
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