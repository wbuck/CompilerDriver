using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record Si : IReg
{
    public static Si Register { get; } = new();
    private Si() { }
    public AssemblyTag Tag => AssemblyTag.Si;
}