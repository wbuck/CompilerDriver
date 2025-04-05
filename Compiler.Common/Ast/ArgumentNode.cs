namespace Compiler.Common.Ast;

public record ArgumentNode(ReadOnlyMemory<char> Name, string Type) : Node
{
    public override NodeType NodeType => NodeType.Argument;
}