namespace Compiler.Generation.Instructions;

public sealed record AllocateStack(int Offset) : IInstruction
{
    public AssemblyTag Tag => AssemblyTag.AllocateStack; 
}