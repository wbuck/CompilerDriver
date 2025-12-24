using System.Diagnostics.CodeAnalysis;
using Compiler.Generation.Instructions;
using Compiler.Tacky.Tac;

namespace Compiler.Generation;

public static class Generator
{
    public static bool TryGenerate(TackyProgram tacky, [NotNullWhen(true)] out Program? program)
    {
        program = null;
        try
        {
            program = Generate(tacky);
            return true;            
        }
        catch (FormatException ex)
        {
            PrintError(ex.Message);
            return false;
        }       
    }

    public static Program Generate(TackyProgram program)
        => Program.Visit(program);
    
    private static void PrintError(ReadOnlySpan<char> error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(error);
        Console.ResetColor();
    }
}