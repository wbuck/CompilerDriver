using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record Ax : IReg
{
    public static Ax Register { get; } = new();
    private Ax() { }
    public AssemblyTag Tag => AssemblyTag.Ax;
}