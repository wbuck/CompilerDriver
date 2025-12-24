namespace Compiler.Generation.Instructions;

public sealed record NotEqual : IConditionCode
{
    public static NotEqual Code { get; } = new();
    private NotEqual() { }
    public AssemblyTag Tag => AssemblyTag.NotEqual;
}