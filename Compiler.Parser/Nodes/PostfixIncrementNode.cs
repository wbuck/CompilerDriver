namespace Compiler.Parser.Nodes;

public sealed record PostfixIncrementNode : IUnaryOperatorNode
{
    public static PostfixIncrementNode Operator { get; } = new();
    private PostfixIncrementNode() { }
    public AstNodeTag Tag => AstNodeTag.PostfixIncrement;
}