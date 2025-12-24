namespace Compiler.Parser.Nodes;

public sealed record BitwiseAndNode : IBitwiseOperatorNode
{
    public static BitwiseAndNode Operator { get; } = new();
    private BitwiseAndNode() { }
    public AstNodeTag Tag => AstNodeTag.BitwiseAnd;
}