namespace Compiler.Tacky.Tac;

public sealed record TackyNot: ITackyUnaryOperator
{
    public static TackyNot Operator { get; } = new();
    private TackyNot() { }
    public TackyTag Tag => TackyTag.Not;
}