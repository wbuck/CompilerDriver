namespace Compiler.Parser.Nodes;

public sealed record GreaterThanOrEqualNode : IBinaryOperatorNode
{
    public static GreaterThanOrEqualNode Operator { get; } = new();
    private GreaterThanOrEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.GreaterThanOrEqual;
}