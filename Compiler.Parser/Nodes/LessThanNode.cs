namespace Compiler.Parser.Nodes;

public sealed record LessThanNode : IBinaryOperatorNode
{
    public static LessThanNode Operator { get; } = new();
    private LessThanNode() { }
    public AstNodeTag Tag => AstNodeTag.LessThan;
}