namespace Compiler.Parser.Nodes;

public sealed record ForNode(
    IForLoopInitializer? Initializer,
    IExpressionNode? Condition, 
    IExpressionNode? Post,
    IStatementNode Body,    
    string? Label = null
) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.For;
}