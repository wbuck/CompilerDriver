namespace Compiler.Parser.Nodes;

public sealed record MultiplicationNode : IBinaryOperatorNode
{
    public static MultiplicationNode Operator { get; } = new();
    private MultiplicationNode() { }
    public AstNodeTag Tag => AstNodeTag.Multiplication;
}