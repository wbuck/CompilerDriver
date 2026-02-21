namespace Compiler.Generation.Instructions;

public sealed record StaticVariable
(
    string Name,
    bool Global,
    int Init
): ITopLevel
{
    public AssemblyTag Tag => AssemblyTag.StaticVariable;
}