namespace Compiler.Parser.Nodes;

public sealed record BinaryNode
(
    IBinaryOperatorNode Operator, 
    IExpressionNode Lhs, 
    IExpressionNode Rhs
) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Binary;
}