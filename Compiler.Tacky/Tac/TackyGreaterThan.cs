namespace Compiler.Tacky.Tac;

public sealed record TackyGreaterThan : ITackyBinaryOperator
{
    public static TackyGreaterThan Operator { get; } = new();
    private TackyGreaterThan() { }
    public TackyTag Tag => TackyTag.GreaterThan;
}