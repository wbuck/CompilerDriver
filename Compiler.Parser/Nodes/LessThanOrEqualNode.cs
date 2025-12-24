namespace Compiler.Parser.Nodes;

public sealed record LessThanOrEqualNode : IBinaryOperatorNode
{
    public static LessThanOrEqualNode Operator { get; } = new();
    private LessThanOrEqualNode() { }
    public AstNodeTag Tag => AstNodeTag.LessThanOrEqual;
}