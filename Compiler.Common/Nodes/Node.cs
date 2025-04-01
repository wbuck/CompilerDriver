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

public record Program(Node Body) : Node
{
    public override NodeType Type => NodeType.Program;
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