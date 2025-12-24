namespace Compiler.Parser.Nodes;

public sealed record ConditionalNode
(
    IExpressionNode Condition, 
    IExpressionNode True, 
    IExpressionNode False
): IExpressionNode
{
    public AstNodeTag Tag => AstNodeTag.Conditional;
}