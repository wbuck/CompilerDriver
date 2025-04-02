using System.Text.Json;

namespace Compiler.Common.Nodes;

public abstract record Node
{
    public abstract NodeType Type { get; }
}