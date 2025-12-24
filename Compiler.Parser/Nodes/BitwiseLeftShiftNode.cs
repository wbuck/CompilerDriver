namespace Compiler.Parser.Nodes;

public sealed record BitwiseLeftShiftNode : IBitwiseOperatorNode
{
    public static BitwiseLeftShiftNode Operator { get; } = new();
    private BitwiseLeftShiftNode() { }
    public AstNodeTag Tag => AstNodeTag.LeftShift;
}