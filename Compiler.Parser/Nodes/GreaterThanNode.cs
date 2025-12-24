namespace Compiler.Parser.Nodes;

public sealed record GreaterThanNode : IBinaryOperatorNode
{
    public static GreaterThanNode Operator { get; } = new();
    private GreaterThanNode() { }
    public AstNodeTag Tag => AstNodeTag.GreaterThan;
}