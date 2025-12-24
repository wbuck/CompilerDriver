namespace Compiler.Tacky.Tac;

public sealed record TackyNegate: ITackyUnaryOperator
{
    public static TackyNegate Operator { get; } = new();
    private TackyNegate() { }
    public TackyTag Tag => TackyTag.Negate;
}