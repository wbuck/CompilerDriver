namespace Compiler.Generation.Instructions;

public sealed record Ret : IInstruction
{
    public static Ret Instruction { get; } = new();
    private Ret() { }
    public AssemblyTag Tag => AssemblyTag.Ret;
}