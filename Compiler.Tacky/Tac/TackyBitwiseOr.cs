namespace Compiler.Tacky.Tac;

public sealed record TackyBitwiseOr : ITackyBitwiseOperator
{
    public static TackyBitwiseOr Operator { get; } = new();
    private TackyBitwiseOr() { }
    public TackyTag Tag => TackyTag.BitwiseOr;
}