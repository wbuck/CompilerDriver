namespace Compiler.Parser.Nodes;

public sealed record NegateNode : IUnaryOperatorNode
{
    public static NegateNode Operator { get; } = new();
    private NegateNode() { }
    public AstNodeTag Tag => AstNodeTag.Negate;
}