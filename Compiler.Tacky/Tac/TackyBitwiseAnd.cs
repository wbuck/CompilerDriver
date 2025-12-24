namespace Compiler.Tacky.Tac;

public sealed record TackyBitwiseAnd : ITackyBitwiseOperator
{
    public static TackyBitwiseAnd Operator { get; } = new();
    private TackyBitwiseAnd() { }
    public TackyTag Tag => TackyTag.BitwiseAnd;
}