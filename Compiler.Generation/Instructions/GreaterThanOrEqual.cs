namespace Compiler.Generation.Instructions;

public sealed record GreaterThanOrEqual : IConditionCode
{
    public static GreaterThanOrEqual Code { get; } = new();
    private GreaterThanOrEqual() { }
    public AssemblyTag Tag => AssemblyTag.GreaterThanOrEqual;
}