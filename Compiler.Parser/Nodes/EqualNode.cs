namespace Compiler.Parser.Nodes;

public sealed record EqualNode : IBinaryOperatorNode
{
    public static EqualNode Operator { get; } = new();
    private EqualNode() { }
    public AstNodeTag Tag => AstNodeTag.Equal;
}