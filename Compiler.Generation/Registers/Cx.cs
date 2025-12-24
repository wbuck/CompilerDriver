using Compiler.Generation.Instructions;

namespace Compiler.Generation.Registers;

public sealed record Cx : IReg
{
    public static Cx Register { get; } = new();
    private Cx() { }
    public AssemblyTag Tag => AssemblyTag.Cx;
}