namespace Compiler.Tacky.Tac;

public sealed record TackyBitwiseXor : ITackyBitwiseOperator
{
    public static TackyBitwiseXor Operator { get; } = new();
    private TackyBitwiseXor() { }
    public TackyTag Tag => TackyTag.BitwiseXor;
}