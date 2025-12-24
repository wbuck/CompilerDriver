namespace Compiler.Parser.Nodes;

public sealed record NotNode : IUnaryOperatorNode
{
    public static NotNode Operator { get; } = new();
    private NotNode() { }
    public AstNodeTag Tag => AstNodeTag.Not;
}