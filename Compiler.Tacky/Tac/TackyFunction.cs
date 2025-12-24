namespace Compiler.Tacky.Tac;

public sealed record TackyFunction
(
    string Name, 
    List<string> Parameters,
    List<ITackyInstruction> Instructions
) : ITackyTag
{
    public TackyTag Tag => TackyTag.Function;
}