namespace Compiler.Tacky.Tac;

public sealed record TackyJumpIfNotZero(ITackyValue Condition, string Target) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.JumpIfNotZero;
}