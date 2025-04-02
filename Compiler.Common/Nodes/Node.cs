using System.Numerics;
using System.Text.Json;

namespace Compiler.Common.Nodes;

public abstract record Node
{
    public abstract NodeType Type { get; }
}

public record Constant<T>(T Value) : Node where T: INumber<T>
{
    public override NodeType Type => NodeType.Constant;
}