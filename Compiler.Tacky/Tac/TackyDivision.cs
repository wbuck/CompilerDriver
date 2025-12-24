namespace Compiler.Tacky.Tac;

public sealed record TackyDivision : ITackyBinaryOperator
{
    public static TackyDivision Operator { get; } = new();
    private TackyDivision() { }
    public TackyTag Tag => TackyTag.Division;
}