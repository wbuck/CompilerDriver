using System.Numerics;

namespace Compiler.Common.Ast;

public record IntegralConstantNode<T>(T Value) : INode where T: IBinaryInteger<T>
{
    public NodeType NodeType => NodeType.Constant;
}