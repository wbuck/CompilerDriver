namespace Compiler.Tacky.Tac;

public sealed record TackyVariable(string Identifier) : ITackyValue
{
    public TackyTag Tag => TackyTag.Variable;
}