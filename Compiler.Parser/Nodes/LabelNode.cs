namespace Compiler.Parser.Nodes;

public sealed record LabelNode(string Name, IStatementNode Statement) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Label;
}