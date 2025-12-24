namespace Compiler.Parser.Nodes;

public sealed record AssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.Assignment;
    public bool IsCompound => false;
}