namespace Compiler.Parser.Nodes;

public sealed record BitwiseAndAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.BitwiseAndAssignment;
    public bool IsCompound => true;
}