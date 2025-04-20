using System.Diagnostics.CodeAnalysis;
using Compiler.Common.Ast;
using Compiler.Common.Generation;
using Compiler.Common.Tacky;

namespace Compiler.Common.Stages;

public static class Generator
{
    public static bool TryGenerate(ProgramNode node, [NotNullWhen(true)] out Program? program)
    {
        program = null;
        try
        {
            program = Generate(node);
            return true;            
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
            return false;
        }       
    }

    public static Program Generate(ProgramNode node)
        => Program.Visit(TackyBase.Visit(node));
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}