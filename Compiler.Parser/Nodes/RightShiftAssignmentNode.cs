namespace Compiler.Parser.Nodes;

public sealed record RightShiftAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.RightShiftAssignment;
    public bool IsCompound => true;
}