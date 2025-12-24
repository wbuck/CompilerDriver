namespace Compiler.Parser.Nodes;

public sealed record LeftShiftAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.LeftShiftAssignment;
    public bool IsCompound => true;
}