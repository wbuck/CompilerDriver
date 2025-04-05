namespace Compiler.Common.Ast;

public record ReturnNode(INode Expression) : INode
{
    public NodeType NodeType => NodeType.Return;
}