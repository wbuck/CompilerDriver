namespace Compiler.Tacky.Tac;

public sealed record TackyProgram(List<ITackyTopLevel> TopLevel) : ITackyTag
{
    public TackyTag Tag => TackyTag.Program;    
}