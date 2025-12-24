namespace Compiler.Parser.Nodes;

public sealed record SwitchNode(
    IExpressionNode Value, 
    IStatementNode Body,
    IReadOnlyList<SwitchLabel>? Cases = null,
    string? Label = null
) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Switch;
}