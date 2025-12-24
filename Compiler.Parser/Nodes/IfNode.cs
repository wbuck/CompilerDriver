namespace Compiler.Parser.Nodes;

public sealed record IfNode
(
    IExpressionNode Condition, 
    IStatementNode Then, 
    IStatementNode? Else = null
) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.If;
}