using System.Numerics;

namespace Compiler.Tacky.Tac;

public sealed record TackyConstant<T>(T Value) : ITackyValue where T : INumber<T>
{
    public TackyTag Tag => TackyTag.Constant;    
}