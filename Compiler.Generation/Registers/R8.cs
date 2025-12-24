using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record R8 : IReg
{
    public static R8 Register { get; } = new();
    private R8() { }
    public AssemblyTag Tag => AssemblyTag.R8;
}