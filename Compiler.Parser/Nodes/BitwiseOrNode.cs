namespace Compiler.Parser.Nodes;

public sealed record BitwiseOrNode : IBitwiseOperatorNode
{
    public static BitwiseOrNode Operator { get; } = new();
    private BitwiseOrNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseOr;
}