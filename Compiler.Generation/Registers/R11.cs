using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record R11 : IReg
{
    public static R11 Register { get; } = new();
    private R11() { }
    public AssemblyTag Tag => AssemblyTag.R11;
}