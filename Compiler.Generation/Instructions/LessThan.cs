namespace Compiler.Generation.Instructions;

public sealed record LessThan : IConditionCode
{
    public static LessThan Code { get; } = new();
    private LessThan() { }
    public AssemblyTag Tag => AssemblyTag.LessThan;
}