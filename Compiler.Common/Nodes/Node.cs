using System.Text;
using System.Text.Json;

namespace Compiler.Common.Nodes;

public enum NodeType
{
    Program, 
    Function, 
    Return, 
    Constant
}

public abstract record Node
{
    public abstract NodeType Type { get; }
}

public record Program(List<Node> Nodes) : Node
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

public record Function(string Name, Node Body) : Node
{
    public override NodeType Type => NodeType.Function;
}

public record Return(Node Expression) : Node
{
    public override NodeType Type => NodeType.Return;
}

public record Constant<T>(T Value) : Node
{
    public override NodeType Type => NodeType.Constant;
}