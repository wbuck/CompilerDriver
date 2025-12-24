namespace Compiler.Parser.Nodes;

public sealed record BitwiseXorNode : IBitwiseOperatorNode
{
    public static BitwiseXorNode Operator { get; } = new();
    private BitwiseXorNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseOr;
}