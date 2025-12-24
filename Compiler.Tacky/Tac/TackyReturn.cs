namespace Compiler.Tacky.Tac;

public sealed record TackyReturn(ITackyValue Value) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Return;
}