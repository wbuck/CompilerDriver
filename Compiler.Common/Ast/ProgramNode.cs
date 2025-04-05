using System.Text;

namespace Compiler.Common.Ast;

public record ProgramNode(List<INode> Nodes) : INode
{
    public NodeType NodeType => NodeType.Program;
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append("Program {");
        builder.Append($" Type = {NodeType},");
        builder.Append($" Nodes = [ ");
        builder.Append(string.Join(", ", Nodes));
        builder.Append(" ] }");
        return builder.ToString();
    }
}