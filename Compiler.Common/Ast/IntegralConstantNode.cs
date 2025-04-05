using System.Numerics;

namespace Compiler.Common.Ast;

public record IntegralConstantNode<T>(T Value) : Node where T: IBinaryInteger<T>
{
    public override NodeType NodeType => NodeType.Constant;
}