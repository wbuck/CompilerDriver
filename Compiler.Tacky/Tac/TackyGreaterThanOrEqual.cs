namespace Compiler.Tacky.Tac;

public sealed record TackyGreaterThanOrEqual : ITackyBinaryOperator
{
    public static TackyGreaterThanOrEqual Operator { get; } = new();
    private TackyGreaterThanOrEqual() { }
    public TackyTag Tag => TackyTag.GreaterThanOrEqual;
}