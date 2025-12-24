namespace Compiler.Parser.Nodes;

public sealed record BreakNode(string? Label = null) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Break;
}