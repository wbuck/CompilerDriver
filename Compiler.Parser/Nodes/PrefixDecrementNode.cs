namespace Compiler.Parser.Nodes;

public sealed record PrefixDecrementNode : IUnaryOperatorNode
{
    public static PrefixDecrementNode Operator { get; } = new();
    private PrefixDecrementNode() { }
    public AstNodeTag Tag => AstNodeTag.PrefixDecrement;
}