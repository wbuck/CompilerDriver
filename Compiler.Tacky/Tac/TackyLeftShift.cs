namespace Compiler.Tacky.Tac;

public sealed record TackyLeftShift : ITackyBitwiseOperator
{
    public static TackyLeftShift Operator { get; } = new();
    private TackyLeftShift() { }
    public TackyTag Tag => TackyTag.LeftShift;
}