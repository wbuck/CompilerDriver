namespace Compiler.Parser.Nodes;

public sealed record DivisionAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.DivisionAssignment;
    public bool IsCompound => true;
}