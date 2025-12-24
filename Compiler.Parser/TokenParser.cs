using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Compiler.Lexer.Tokens;
using Compiler.Parser.Nodes;

namespace Compiler.Parser;

public static class TokenParser
{
    public static bool TryParse(
        in List<IToken> tokens, 
        ReadOnlyMemory<char> fileContent, 
        [NotNullWhen(true)] out ProgramNode? node)
    {
        try
        {
            node = Parse(tokens, fileContent);
            return true;
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);            
        }
        node = null;
        return false;
    }
    
    public static ProgramNode Parse(in List<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var input = CollectionsMarshal.AsSpan(tokens);
        return ProgramNode.Parse(ref input, fileContent);                 
    }

    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}