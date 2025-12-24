namespace Compiler.Generation.Instructions;

public sealed record Cdq : IInstruction
{
    public static Cdq Instruction { get; } = new();
    private Cdq() { }
    public AssemblyTag Tag => AssemblyTag.Cdq;   
}