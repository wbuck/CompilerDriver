namespace Compiler.Tacky.Tac;

public sealed record TackyMultiplication : ITackyBinaryOperator
{
    public static TackyMultiplication Operator { get; } = new();
    private TackyMultiplication() { }
    public TackyTag Tag => TackyTag.Multiplication;
}