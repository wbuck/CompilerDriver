namespace Compiler.Parser.Nodes;

public sealed record BitwiseRightShiftNode : IBitwiseOperatorNode
{
    public static BitwiseRightShiftNode Operator { get; } = new();
    private BitwiseRightShiftNode() { }
    public AstNodeTag Tag => AstNodeTag.RightShift;
}