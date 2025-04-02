namespace Compiler.Common.Nodes;

public record Function(string Name, Node Body) : Node
{
    public override NodeType Type => NodeType.Function;
}