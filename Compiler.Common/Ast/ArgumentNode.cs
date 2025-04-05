namespace Compiler.Common.Ast;

public record ArgumentNode(string Name, string Type) : Node
{
    public override NodeType NodeType => NodeType.Argument;
}