namespace Compiler.Tacky.Tac;

public sealed record TackyBitwise(ITackyBitwiseOperator Operator, ITackyValue Lhs, ITackyValue Rhs, ITackyValue Destination)
    : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Bitwise;
}