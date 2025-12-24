namespace Compiler.Parser.Nodes;

public sealed record RemainderNode : IBinaryOperatorNode
{
    public static RemainderNode Operator { get; } = new();
    private RemainderNode() { }
    public AstNodeTag Tag => AstNodeTag.Remainder;
}