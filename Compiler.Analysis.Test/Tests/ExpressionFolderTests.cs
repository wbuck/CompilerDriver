using System.Runtime.InteropServices;
using Compiler.Analysis.Helpers;
using Compiler.Analysis.Validators;
using Compiler.Lexer;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;

namespace Compiler.Analysis.Test.Tests;

public class ExpressionFolderTests
{
    [Theory]
    [InlineData("1 + 2", 3)]
    [InlineData("6 % 5 - 1", 0)]
    [InlineData("4 / 2 * 10", 20)]
    [InlineData("4 / 2 + 10 * 7", 72)]
    [InlineData("24 / (2 + 10) * 7", 14)]
    [InlineData("10 << 1", 20)]
    [InlineData("10 >> 1", 5)]
    [InlineData("5 & 1", 1)]
    [InlineData("5 | 2", 7)]
    [InlineData("5 ^ 1", 4)]
    [InlineData("!1", 0)]
    [InlineData("!0", 1)]
    [InlineData("~1", -2)]
    [InlineData("-100", -100)]
    [InlineData("24 / (2 + 10) * (5 << 1)", 20)]
    [InlineData("24 / (2 + 10) * (5 << !0)", 20)]
    [InlineData("20 / ((2 + 2) * 5) * 7", 7)]
    public void ShouldFoldExpression(string expression, int result)
    {
        var node = Parse
        (
            $$"""
            int main(void) {
                int x = {{expression}};
            }                                  
            """
        );        
        Assert.Equivalent
        (
            new ConstantNode<int>(result), 
            ExpressionFolder.FoldExpression(node.Initializer!), 
            true
        );
    }
    
    private static VariableDeclarationNode Parse(string fileContent)
    {
        var tokens = CollectionsMarshal.AsSpan(GetTokens(fileContent));
        var validator = new SemanticValidator();
        var node = validator.Validate(ProgramNode.Parse(ref tokens, fileContent.AsMemory()));
        return (VariableDeclarationNode)((FunctionDeclarationNode)node.Nodes[0]).Body!.Items[0];
    }
    
    private static List<IToken> GetTokens(ReadOnlySpan<char> fileContent)
    {
        Assert.True(LexicalAnalyzer.TryTokenize(fileContent, out var tokens));
        return tokens;
    }
}
