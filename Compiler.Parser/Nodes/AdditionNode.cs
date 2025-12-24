namespace Compiler.Parser.Nodes;

public sealed record AdditionNode : IBinaryOperatorNode
{
    public static AdditionNode Operator { get; } = new();
    private AdditionNode() { }
    public AstNodeTag Tag => AstNodeTag.Addition;
}