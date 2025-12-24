using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record R9 : IReg
{
    public static R9 Register { get; } = new();
    private R9() { }
    public AssemblyTag Tag => AssemblyTag.R9;
}