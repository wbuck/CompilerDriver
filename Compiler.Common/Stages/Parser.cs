using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Compiler.Common.Ast;
using Compiler.Common.Tokens;

namespace Compiler.Common.Stages;

public static class Parser
{
    public static bool TryParse(in List<IToken> tokens, ReadOnlyMemory<char> fileContent, [NotNullWhen(true)] out INode? node)
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
    
    public static INode Parse(in List<IToken> tokens, ReadOnlyMemory<char> fileContent)
    {
        var input = CollectionsMarshal.AsSpan(tokens);
        return ProgramNode.Parse(ref input, fileContent)!;                 
    }

    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}