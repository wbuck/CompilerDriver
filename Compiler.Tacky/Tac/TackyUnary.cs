namespace Compiler.Tacky.Tac;

public sealed record TackyUnary(ITackyUnaryOperator Operator, ITackyValue Source, ITackyValue Destination) 
    : ITackyInstruction
{
    public TackyTag Tag  => TackyTag.Unary;
}