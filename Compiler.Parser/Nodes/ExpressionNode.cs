namespace Compiler.Parser.Nodes;

public sealed record ExpressionNode(IExpressionNode Expression) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Expression;
}