namespace Compiler.Parser.Nodes;

public sealed record DivisionNode : IBinaryOperatorNode
{
    public static DivisionNode Operator { get; } = new();
    private DivisionNode() { }
    public AstNodeTag Tag => AstNodeTag.Division;
}