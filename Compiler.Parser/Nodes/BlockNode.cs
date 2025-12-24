namespace Compiler.Parser.Nodes;

public sealed record BlockNode(List<IBlockItem> Items) : IAstNodeTag
{
    public AstNodeTag Tag => AstNodeTag.Block;
}