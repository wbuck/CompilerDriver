namespace Compiler.Parser.Nodes;

public sealed record RemainderAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.RemainderAssignment;
    public bool IsCompound => true;
}