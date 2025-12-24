using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record Dx : IReg
{
    public static Dx Register { get; } = new();
    private Dx() { }
    public AssemblyTag Tag => AssemblyTag.Dx;
}