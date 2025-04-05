namespace Compiler.Common.Ast;

public abstract record Node
{
    public abstract NodeType NodeType { get; }
}