namespace Compiler.Parser.Nodes;

public sealed record NullNode : IStatementNode
{
    public static NullNode Statement { get; } = new();
    private NullNode() { }
    public AstNodeTag Tag => AstNodeTag.Null;
}