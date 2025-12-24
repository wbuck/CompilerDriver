namespace Compiler.Tacky.Tac;

public sealed record TackyAddition : ITackyBinaryOperator
{
    public static TackyAddition Operator { get; } = new();
    private TackyAddition() { }
    public TackyTag Tag  => TackyTag.Addition;
}