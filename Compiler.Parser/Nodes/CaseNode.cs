namespace Compiler.Parser.Nodes;

public sealed record CaseNode
(
    IExpressionNode ConstantExpression,
    IStatementNode Statement,
    string? Label = null
) : ISwitchLabelNode
{
    public AstNodeTag Tag => AstNodeTag.Case;
}