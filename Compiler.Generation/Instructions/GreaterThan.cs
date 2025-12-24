namespace Compiler.Generation.Instructions;

public sealed record GreaterThan : IConditionCode
{
    public static GreaterThan Code { get; } = new();
    private GreaterThan() { }
    public AssemblyTag Tag => AssemblyTag.GreaterThan;
}