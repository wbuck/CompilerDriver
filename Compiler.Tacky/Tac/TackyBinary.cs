namespace Compiler.Tacky.Tac;

public sealed record TackyBinary
(
    ITackyBinaryOperator Operator, 
    ITackyValue Lhs, 
    ITackyValue Rhs, 
    ITackyValue Destination
) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.Binary;
}