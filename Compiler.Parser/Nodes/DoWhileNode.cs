namespace Compiler.Parser.Nodes;

public sealed record DoWhileNode
(
    IStatementNode Body, 
    IExpressionNode Condition, 
    string? Label = null
) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.DoWhile;
}