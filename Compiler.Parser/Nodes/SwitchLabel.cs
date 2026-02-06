namespace Compiler.Parser.Nodes;

public readonly record struct SwitchLabel
(
    string Label, 
    ConstantNode<int>? Value
);