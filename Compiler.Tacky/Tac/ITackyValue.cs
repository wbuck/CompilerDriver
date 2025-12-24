namespace Compiler.Tacky.Tac;

public interface ITackyValue : ITackyTag
{
    public static TackyConstant<int> True { get; } = new(1);
    public static TackyConstant<int> False { get; } = new(0);
    public static TackyConstant<int> One { get; } = new(1);
}