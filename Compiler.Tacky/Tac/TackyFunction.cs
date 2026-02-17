namespace Compiler.Tacky.Tac;

public sealed record TackyFunction
(
    string Name, 
    bool Global,
    List<string> Parameters,
    List<ITackyInstruction> Instructions
): ITackyTopLevel
{
    public TackyTag Tag => TackyTag.Function;
}