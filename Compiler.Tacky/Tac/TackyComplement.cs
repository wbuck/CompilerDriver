namespace Compiler.Tacky.Tac;

public sealed record TackyComplement: ITackyUnaryOperator
{
    public static TackyComplement Operator { get; } = new();
    private TackyComplement() { }
    public TackyTag Tag => TackyTag.Complement;
}