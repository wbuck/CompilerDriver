namespace Compiler.Parser.Nodes;

public sealed record BitwiseXorAssignmentNode(IExpressionNode Lhs, IExpressionNode Rhs) : IAssignmentNode
{
    public AstNodeTag Tag => AstNodeTag.BitwiseXorAssignment;
    public bool IsCompound => true;
}