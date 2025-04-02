using System.Numerics;

namespace Compiler.Common.Nodes;

public record IntegralConstantNode<T>(T Value) : Node where T: IBinaryInteger<T>
{
    public override NodeType Type => NodeType.Constant;
}