namespace Compiler.Common.Nodes;

public record FunctionNode(string Name, Node Body) : Node
{
    public override NodeType Type => NodeType.Function;
}