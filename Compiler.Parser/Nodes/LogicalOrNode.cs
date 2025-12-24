namespace Compiler.Parser.Nodes;

public sealed record LogicalOrNode : IBinaryOperatorNode
{
    public static LogicalOrNode Operator { get; } = new();
    private LogicalOrNode() { }
    public AstNodeTag Tag => AstNodeTag.LogicalOr;
}