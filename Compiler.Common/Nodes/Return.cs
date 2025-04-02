namespace Compiler.Common.Nodes;

public record Return(Node Expression) : Node
{
    public override NodeType Type => NodeType.Return;
}