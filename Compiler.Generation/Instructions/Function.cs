namespace Compiler.Generation.Instructions;

public sealed record Function
(
    string Name, 
    bool Global,
    List<IInstruction> Instructions
): ITopLevel
{
    public AssemblyTag Tag => AssemblyTag.Function;   
}