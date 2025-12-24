namespace Compiler.Parser.Nodes;

public sealed record AdditionAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.AdditionAssignment;
    public bool IsCompound => true;
}