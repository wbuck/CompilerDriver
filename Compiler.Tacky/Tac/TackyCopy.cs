namespace Compiler.Tacky.Tac;

public sealed record TackyCopy(ITackyValue Source, ITackyValue Destination) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Copy;
}