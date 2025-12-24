namespace Compiler.Tacky.Tac;

public sealed record TackyEqual : ITackyBinaryOperator
{
    public static TackyEqual Operator { get; } = new();
    private TackyEqual() { }
    public TackyTag Tag => TackyTag.Equal;
}