namespace Compiler.Parser.Nodes;

public sealed record MultiplicationAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.MultiplicationAssignment;
    public bool IsCompound => true;
}