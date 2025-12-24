namespace Compiler.Tacky.Tac;

public sealed record TackySubtraction : ITackyBinaryOperator
{
    public static TackySubtraction Operator { get; } = new();
    private TackySubtraction() { }
    public TackyTag Tag   => TackyTag.Subtraction;
}