namespace Compiler.Parser.Nodes;

public sealed record CompoundNode(BlockNode Block) : IStatementNode
{
    public AstNodeTag Tag => AstNodeTag.Compound;
}