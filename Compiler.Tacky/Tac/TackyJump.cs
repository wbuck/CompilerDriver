namespace Compiler.Tacky.Tac;

public sealed record TackyJump(string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Jump;
}