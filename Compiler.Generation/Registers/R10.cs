using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record R10 : IReg
{
    public static R10 Register { get; } = new();
    private R10() { }
    public AssemblyTag Tag => AssemblyTag.R10;
}