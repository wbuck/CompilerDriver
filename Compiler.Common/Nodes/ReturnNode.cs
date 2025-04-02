namespace Compiler.Common.Nodes;

public record ReturnNode(Node Expression) : Node
{
    public override NodeType Type => NodeType.Return;
}