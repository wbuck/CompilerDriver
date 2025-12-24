namespace Compiler.Tacky.Tac;

public sealed record TackyLessThan : ITackyBinaryOperator
{
    public static TackyLessThan Operator { get; } = new();
    private TackyLessThan() { }
    public TackyTag Tag => TackyTag.LessThan;
}