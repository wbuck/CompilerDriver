namespace Compiler.Tacky.Tac;

public sealed record TackyRightShift : ITackyBitwiseOperator
{
    public static TackyRightShift Operator { get; } = new();
    private TackyRightShift() { }
    public TackyTag Tag => TackyTag.RightShift;
}