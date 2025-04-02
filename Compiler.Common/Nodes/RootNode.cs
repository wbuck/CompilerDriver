using System.Text;

namespace Compiler.Common.Nodes;

public record RootNode(List<Node> Nodes) : Node
{
    
    public override NodeType Type => NodeType.Program;
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append("Program {");
        builder.Append($" Type = {Type}");
        builder.Append($" Node = {Type}");
        foreach (var node in Nodes)
        {
            builder.Append(node);
        }
        builder.Append('}');
        return builder.ToString();
    }
}