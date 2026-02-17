namespace Compiler.Tacky.Tac;

public sealed record TackyStaticVariable
(
    string Identifier,
    bool Global,
    int Init
) : ITackyTopLevel
{
    public TackyTag Tag => TackyTag.StaticVariable;
}