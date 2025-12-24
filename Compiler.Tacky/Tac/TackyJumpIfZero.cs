namespace Compiler.Tacky.Tac;

public sealed record TackyJumpIfZero(ITackyValue Condition, string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.JumpIfZero;
}