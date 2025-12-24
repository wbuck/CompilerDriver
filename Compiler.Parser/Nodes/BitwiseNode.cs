namespace Compiler.Parser.Nodes;

public sealed record BitwiseNode
(
    IBitwiseOperatorNode Operator, 
    IExpressionNode Lhs, 
    IExpressionNode Rhs
) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Bitwise;
}