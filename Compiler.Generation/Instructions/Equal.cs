namespace Compiler.Generation.Instructions;

public sealed record Equal : IConditionCode
{
    public static Equal Code { get; } = new();
    private Equal() { }
    public AssemblyTag Tag => AssemblyTag.Equal;
}