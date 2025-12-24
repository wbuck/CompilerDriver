namespace Compiler.Tacky.Tac;

public sealed record TackyLabel(string Identifier) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Label;
}