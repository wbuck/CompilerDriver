namespace Compiler.Tacky.Tac;

public sealed record TackyLessThanOrEqual : ITackyBinaryOperator
{
    public static TackyLessThanOrEqual Operator { get; } = new();
    private TackyLessThanOrEqual() { }
    public TackyTag Tag => TackyTag.LessThanOrEqual;
}