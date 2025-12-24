namespace Compiler.Parser.Nodes;

public sealed record ComplementNode : IUnaryOperatorNode
{
    public static ComplementNode Operator { get; } = new();
    private ComplementNode() { }
    public AstNodeTag Tag => AstNodeTag.Complement;
}