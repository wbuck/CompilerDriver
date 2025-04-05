namespace Compiler.Common.Ast;

public record ReturnNode(Node Expression) : Node
{
    public override NodeType NodeType => NodeType.Return;
}