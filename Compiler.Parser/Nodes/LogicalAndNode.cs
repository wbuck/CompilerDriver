namespace Compiler.Parser.Nodes;

public sealed record LogicalAndNode : IBinaryOperatorNode
{
    public static LogicalAndNode Operator { get; } = new();
    private LogicalAndNode() { }
    public AstNodeTag Tag => AstNodeTag.LogicalAnd;
}