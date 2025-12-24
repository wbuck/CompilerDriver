namespace Compiler.Tacky.Tac;

public sealed record TackyRemainder : ITackyBinaryOperator
{
    public static TackyRemainder Operator { get; } = new();
    private TackyRemainder() { }
    public TackyTag Tag => TackyTag.Remainder;
}