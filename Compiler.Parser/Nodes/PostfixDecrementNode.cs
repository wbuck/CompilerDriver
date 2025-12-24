namespace Compiler.Parser.Nodes;

public sealed record PostfixDecrementNode : IUnaryOperatorNode
{
    public static PostfixDecrementNode Operator { get; } = new();
    private PostfixDecrementNode() { }
    public AstNodeTag Tag => AstNodeTag.PostfixDecrement;
}