namespace Compiler.Parser.Nodes;

public sealed record WhileNode
(
    IExpressionNode Condition, 
    IStatementNode Body, 
    string? Label = null
) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.While;
}