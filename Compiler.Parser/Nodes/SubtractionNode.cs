namespace Compiler.Parser.Nodes;

public sealed record SubtractionNode : IBinaryOperatorNode
{
    public static SubtractionNode Operator { get; } = new();
    private SubtractionNode() { }
    public AstNodeTag Tag => AstNodeTag.Subtraction;
}