namespace Compiler.Parser.Nodes;

public sealed record DefaultNode
(
    IStatementNode Statement, 
    string? Label = null
) : ISwitchLabelNode
{
    public AstNodeTag Tag => AstNodeTag.Default;
}