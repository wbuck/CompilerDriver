using System.Numerics;

namespace Compiler.Parser.Nodes;

public sealed record ConstantNode<T>(T Value) : IConstantNode where T : INumber<T>
{
    public AstNodeTag Tag => AstNodeTag.Constant;
}