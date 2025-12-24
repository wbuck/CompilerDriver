namespace Compiler.Parser.Nodes;

public sealed record UnaryNode(IUnaryOperatorNode Operator, IExpressionNode Expression) : IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Unary;
}