namespace Compiler.Tacky.Tac;

public sealed record TackyNotEqual : ITackyBinaryOperator
{
    public static TackyNotEqual Operator { get; } = new();
    private TackyNotEqual() { }
    public TackyTag Tag => TackyTag.NotEqual;
}