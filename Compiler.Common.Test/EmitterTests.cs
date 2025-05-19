using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Stages;
using Compiler.Common.Tacky;
using Compiler.Common.Test.Data.Emitter;
using Compiler.Common.Tokens;
using Xunit.Abstractions;

namespace Compiler.Common.Test;

public class EmitterTests(ITestOutputHelper output)
{    
    [Theory]
    [ClassData(typeof(BinaryOperatorData))]
    public void EmitBinaryOperatorShouldSuccessfullyConvertProgramToAssembly(string fileContent, string[] expected)
    {
        var compiled = GetCompiled(fileContent.AsMemory());
        Assert.Equivalent(expected, compiled, strict: true);
    }

    [Theory]
    [ClassData(typeof(UnaryOperatorData))]
    public void EmitUnaryOperatorShouldSuccessfullyConvertProgramToAssembly(string fileContent, string[] expected)
    {
        var compiled = GetCompiled(fileContent.AsMemory());
        Assert.Equivalent(expected, compiled, strict: true);
    }

    [Theory]
    [ClassData(typeof(BitwiseOperatorData))]
    public void EmitBitwiseOperatorShouldSuccessfullyConvertProgramToAssembly(string fileContent, string[] expected)
    {
        var compiled = GetCompiled(fileContent.AsMemory());
        Assert.Equivalent(expected, compiled, strict: true);
    }
    
    private List<string> GetCompiled(ReadOnlyMemory<char> fileContent)
    {
        var compiled = Emitter.Emit(GetAssembly(fileContent));
        var result = compiled
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .ToList();
        
        output.WriteLine("Input:");
        output.WriteLine(fileContent.ToString());
        output.WriteLine(string.Empty);
        output.WriteLine("Actual Result:");
        result.ForEach(i => output.WriteLine(i.ToString()));
        return result;
    }
    
    private static Program GetAssembly(ReadOnlyMemory<char> fileContent)
        => Program.Visit(GetTacky(fileContent));
    
    private static TackyProgram GetTacky(ReadOnlyMemory<char> fileContent)
        => TackyProgram.Visit(GetAst(fileContent));
    
    private static ProgramNode GetAst(ReadOnlyMemory<char> fileContent)
        => Parser.Parse(GetTokens(fileContent.Span), fileContent);
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(Lexer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}