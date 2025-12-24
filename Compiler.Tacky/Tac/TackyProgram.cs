namespace Compiler.Tacky.Tac;

public sealed record TackyProgram(List<TackyFunction> Functions) : ITackyTag
{
    public TackyTag Tag => TackyTag.Program;    
}