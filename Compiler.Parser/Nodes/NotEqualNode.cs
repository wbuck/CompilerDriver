namespace Compiler.Parser.Nodes;

public sealed record NotEqualNode : IBinaryOperatorNode
{
    public static NotEqualNode Operator { get; } = new();
    private NotEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.NotEqual;
}