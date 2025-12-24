namespace Compiler.Parser.Nodes;

public sealed record GotoNode(string Label) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Goto;
}