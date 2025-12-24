using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record Di : IReg
{
    public static Di Register { get; } = new();
    private Di() { }
    public AssemblyTag Tag => AssemblyTag.Di;
}