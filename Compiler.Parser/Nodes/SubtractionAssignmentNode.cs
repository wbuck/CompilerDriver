namespace Compiler.Parser.Nodes;

public sealed record SubtractionAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.SubtractionAssignment;
    public bool IsCompound => true;
}