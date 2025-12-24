namespace Compiler.Generation.Instructions;

public sealed record Function
(
    string Name, 
    List<IInstruction> Instructions
): IAssembly
{
    public AssemblyTag Tag => AssemblyTag.Function;   
}