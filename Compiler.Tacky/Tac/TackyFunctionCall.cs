namespace Compiler.Tacky.Tac;

public sealed record TackyFunctionCall
(
    string Identifier,
    List<ITackyValue> Arguments,
    ITackyValue Destination
) : ITackyInstruction
{
    public TackyTag Tag => TackyTag.FunctionCall;
}