namespace Compiler.Generation.Instructions;

public sealed record LessThanOrEqual : IConditionCode
{
    public static LessThanOrEqual Code { get; } = new();
    private LessThanOrEqual() { }
    public AssemblyTag Tag => AssemblyTag.LessThanOrEqual;
}