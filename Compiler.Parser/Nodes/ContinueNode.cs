namespace Compiler.Parser.Nodes;

public sealed record ContinueNode(string? Label = null) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Continue;
}