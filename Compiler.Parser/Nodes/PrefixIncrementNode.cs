namespace Compiler.Parser.Nodes;

public sealed record PrefixIncrementNode : IUnaryOperatorNode
{
    public static PrefixIncrementNode Operator { get; } = new();
    private PrefixIncrementNode() { }
    public AstNodeTag Tag => AstNodeTag.PrefixIncrement;
}