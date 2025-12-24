namespace Compiler.Parser.Nodes;

public sealed record BitwiseOrAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.BitwiseOrAssignment;
    public bool IsCompound => true;
}